using AuthenticationService.Keycloak.KeycloakHttpClient;
using AuthenticationService.Keycloak.KeycloakHttpClient.Dto;
using CSharpFunctionalExtensions;
using Shared.CQRS;
using Shared.ResultPattern;

namespace AuthenticationService.UseCases.RefreshToken;

public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, Result<TokenPairResponse, Error>>
{
    private readonly IKeycloakClient _keycloakClient;

    public RefreshTokenHandler(IKeycloakClient keycloakClient)
    {
        _keycloakClient = keycloakClient;
    }

    public async Task<Result<TokenPairResponse, Error>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _keycloakClient.RefreshTokensAsync(
            request.RefreshToken,
            cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }
}
