using AuthenticationService.Keycloak.KeycloakHttpClient.Dto;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Shared.ResultPattern;
using StackExchange.Redis;
using System.Net.Http.Headers;

namespace AuthenticationService.Keycloak.KeycloakHttpClient;

public class KeycloakClient : IKeycloakClient
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakOptions _keycloakOptions;
    private readonly IDatabaseAsync _redis;
    private readonly SemaphoreSlim _semaphore; 
    private readonly string CreateUserPath;
    private readonly string GetAdminAccessTokenPath;
    private const string REDIS_ADMIN_ACCESS_TOKEN = "keycloak:admin:token";

    public KeycloakClient(
        HttpClient httpClient,
        IConnectionMultiplexer multiplexer,
        IOptions<KeycloakOptions> options)
    {
        _keycloakOptions = options.Value;
        _httpClient = httpClient;
        _redis = multiplexer.GetDatabase();
        _semaphore = new SemaphoreSlim(1, 1);
        CreateUserPath = $"/admin/realms/{_keycloakOptions.Realm}/users";
        GetAdminAccessTokenPath = $"/realms/{_keycloakOptions.Realm}/protocol/openid-connect/token";
    }

    public async Task<Result<string, Error>> CreateUserAsync(CreateUserDto dto, string accessToken, CancellationToken cancellationToken = default)
    {
        var createUserBody = new CreateUserBody(
            dto.Username, 
            dto.Email, 
            dto.FirstName, 
            dto.Lastname, 
            dto.Password);

        var json = JsonConvert.SerializeObject(createUserBody, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        });

        var content = new StringContent(json, new MediaTypeHeaderValue("application/json"));
        var request = new HttpRequestMessage(HttpMethod.Post, CreateUserPath)
        {
            Content = content
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var userId = response.Headers.Location?.Segments?.LastOrDefault()?.Trim('/');
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new Error("UserId is null or white space", "create-user", "keycloak-client");
            }
            return userId;
        }

        return new Error(
            response.ReasonPhrase ?? await response.Content.ReadAsStringAsync(cancellationToken), 
            "create-user", 
            "keycloak-client");
    }

    public async Task<Result<string, Error>> GetAdminAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var cachedToken = await _redis.StringGetAsync(REDIS_ADMIN_ACCESS_TOKEN);
        if (!cachedToken.IsNullOrEmpty && cachedToken.HasValue)
            return cachedToken.ToString();

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            cachedToken = await _redis.StringGetAsync(REDIS_ADMIN_ACCESS_TOKEN);
            if (!cachedToken.IsNullOrEmpty && cachedToken.HasValue)
                return cachedToken.ToString();

            var formData = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "client_credentials"),
                new("client_id", _keycloakOptions.ClientId),
                new("client_secret", _keycloakOptions.ClientSecret)
            };
            using var content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.PostAsync(GetAdminAccessTokenPath, content, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                try
                {
                    var tokenResponse = JsonConvert.DeserializeObject<AdminTokenResponse>(json);
                    if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                        return new Error("Access token is null", "get-admin-access-token", "keycloak-client");

                    if (tokenResponse.ExpiresIn == 0)
                        return new Error("Expires in is default", "get-admin-access-token", "keycloak-client");

                    var expiredTime = TimeSpan.FromSeconds(int.Max(tokenResponse.ExpiresIn - 10, 1));

                    await _redis.StringSetAsync(
                        [new(REDIS_ADMIN_ACCESS_TOKEN, tokenResponse.AccessToken)],
                        expiry: expiredTime);

                    return tokenResponse.AccessToken;
                }
                catch (JsonException)
                {
                    return new Error("Failed parse JSON", "get-admin-access-token", "keycloak-client");
                }
            }
            return new Error(
                response.ReasonPhrase ?? await response.Content.ReadAsStringAsync(cancellationToken),
                "get-admin-access-token",
                "keycloak-client");
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
