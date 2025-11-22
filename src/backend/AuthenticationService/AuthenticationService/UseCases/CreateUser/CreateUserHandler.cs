using AuthenticationService.Keycloak.KeycloakHttpClient;
using AuthenticationService.Keycloak.KeycloakHttpClient.Dto;
using CSharpFunctionalExtensions;
using Shared.CQRS;
using Shared.ResultPattern;

namespace AuthenticationService.UseCases.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, Result<string, Error>>
{
    private readonly IKeycloakClient _keycloakClient;

    public CreateUserHandler(IKeycloakClient keycloakClient)
    {
        _keycloakClient = keycloakClient;
    }

    public async Task<Result<string, Error>> Handle(
        CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        var accessToken = await _keycloakClient.GetAdminAccessTokenAsync(cancellationToken);
        if (accessToken.IsFailure)
            return accessToken.Error;

        var content = new CreateUserDto(request.Username, request.Email, request.FirstName, request.LastName, request.Password);
        var result = await _keycloakClient.CreateUserAsync(content, accessToken.Value, cancellationToken);
        if (result.IsFailure)
            return result.Error;

        return result.Value!;
    }
}
