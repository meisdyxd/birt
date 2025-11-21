using AuthenticationService.Keycloak.KeycloakHttpClient.Dto;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Shared.ResultPattern;
using System.Net.Http.Headers;

namespace AuthenticationService.Keycloak.KeycloakHttpClient;

public class KeycloakClient : IKeycloakClient
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakOptions _keycloakOptions;
    private readonly string CreateUserPath;
    private readonly string GetAdminAccessTokenPath;

    public KeycloakClient(
        HttpClient httpClient,
        IOptions<KeycloakOptions> options)
    {
        _keycloakOptions = options.Value;
        _httpClient = httpClient;
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

        Console.WriteLine($"URL: {CreateUserPath}");
        Console.WriteLine($"Body: {json}");
        Console.WriteLine($"Auth: Bearer {accessToken}");

        var response = await _httpClient.SendAsync(request, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Response: {responseContent}");

        if (response.IsSuccessStatusCode)
            return response.Headers.Location?.Segments?.LastOrDefault()?.Trim('/')!;

        return new Error(
            response.ReasonPhrase ?? await response.Content.ReadAsStringAsync(cancellationToken), 
            "create-user", 
            "keycloak-client");
    }

    public async Task<Result<string, Error>> GetAdminAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var collections = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", _keycloakOptions.ClientId),
            new("client_secret", _keycloakOptions.ClientSecret)
        };
        var content = new FormUrlEncodedContent(collections);

        var response = await _httpClient.PostAsync(GetAdminAccessTokenPath, content, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            var jObject = JObject.Parse(json);
            if (jObject.TryGetValue("access_token", out var jToken))
            {
                var accessToken = jToken.Value<string>();
                if (accessToken == null)
                    return new Error(
                        "Access token is null", 
                        "get-admin-access-token", 
                        "keycloak-client");
                return accessToken;
            }
        }
        return new Error(
            response.ReasonPhrase ?? await response.Content.ReadAsStringAsync(cancellationToken), 
            "get-admin-access-token", 
            "keycloak-client");
    }
}
