using AuthenticationService.Keycloak.KeycloakHttpClient.Dto;
using CSharpFunctionalExtensions;
using Shared.ResultPattern;

namespace AuthenticationService.Keycloak.KeycloakHttpClient;

public interface IKeycloakClient
{
    Task<Result<string, Error>> CreateUserAsync(CreateUserDto dto, string accessToken, CancellationToken cancellationToken = default);
    Task<Result<string, Error>> GetAdminAccessTokenAsync(CancellationToken cancellationToken = default);
}
