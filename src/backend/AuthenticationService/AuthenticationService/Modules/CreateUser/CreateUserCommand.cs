using CSharpFunctionalExtensions;
using Shared.CQRS;
using Shared.ResultPattern;

namespace AuthenticationService.Modules.CreateUser;

public sealed record CreateUserCommand : ICommand<Result<string, Error>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Password { get; set; }
    public DateTime Birthdate { get; set; }
    public string Sex { get; set; }
    public string Country { get; set; }
}
