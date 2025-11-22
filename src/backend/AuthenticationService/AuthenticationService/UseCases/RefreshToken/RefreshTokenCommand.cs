using AuthenticationService.Keycloak.KeycloakHttpClient.Dto;
using CSharpFunctionalExtensions;
using Shared.CQRS;
using Shared.ResultPattern;

namespace AuthenticationService.UseCases.RefreshToken;

public sealed record RefreshTokenCommand : ICommand<Result<TokenPairResponse, Error>>
{
    public string RefreshToken { get; init; } = default!;
}