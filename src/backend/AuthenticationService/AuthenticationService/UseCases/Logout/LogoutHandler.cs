using AuthenticationService.Keycloak.KeycloakHttpClient;
using CSharpFunctionalExtensions;
using Shared.CQRS;
using Shared.ResultPattern;

namespace AuthenticationService.UseCases.Logout;

public class LogoutHandler : ICommandHandler<LogoutCommand, UnitResult<Error>>
{
    private readonly IKeycloakClient _keycloakClient;

    public LogoutHandler(IKeycloakClient keycloakClient)
    {
        _keycloakClient = keycloakClient;
    }

    public async Task<UnitResult<Error>> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _keycloakClient.LogoutAsync(request.RefreshToken, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return UnitResult.Success<Error>();
    }
}
