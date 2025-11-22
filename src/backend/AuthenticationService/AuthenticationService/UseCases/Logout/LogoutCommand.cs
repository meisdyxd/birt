using CSharpFunctionalExtensions;
using Shared.CQRS;
using Shared.ResultPattern;

namespace AuthenticationService.UseCases.Logout;

public sealed record LogoutCommand : ICommand<UnitResult<Error>>
{
    public string RefreshToken { get; init; } = default!;
}