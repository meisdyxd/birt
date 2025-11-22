using AuthenticationService.Keycloak.KeycloakHttpClient.Dto;
using CSharpFunctionalExtensions;
using Shared.CQRS;
using Shared.ResultPattern;

namespace AuthenticationService.UseCases.Login;

public sealed record LoginCommand : ICommand<Result<TokenPairResponse, Error>>
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}
