using AuthenticationService.Keycloak.KeycloakHttpClient;
using AuthenticationService.Keycloak.KeycloakHttpClient.Dto;
using CSharpFunctionalExtensions;
using Shared.CQRS;
using Shared.ResultPattern;

namespace AuthenticationService.UseCases.Login;

public class LoginHandler : ICommandHandler<LoginCommand, Result<TokenPairResponse, Error>>
{
    private readonly IKeycloakClient _keycloakClient;

    public LoginHandler(IKeycloakClient keycloakClient)
    {
        _keycloakClient = keycloakClient;
    }

    public async Task<Result<TokenPairResponse, Error>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _keycloakClient.RequestTokenPairAsync(
            request.Username,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
            return result.Error;

        return result.Value;
    }
}
