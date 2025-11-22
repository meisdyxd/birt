using AuthenticationService.UseCases.CreateUser;
using AuthenticationService.UseCases.Login;
using AuthenticationService.UseCases.RefreshToken;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserCommand request,
        [FromServices] CreateUserHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request, cancellationToken);
        if (response.IsFailure)
            return BadRequest(response.Error);
        return Ok(response.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request, cancellationToken);
        if (response.IsFailure)
            return Unauthorized(response.Error);

        return Ok(response.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenCommand request,
        [FromServices] RefreshTokenHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request, cancellationToken);
        if (response.IsFailure)
            return Unauthorized(response.Error);

        return Ok(response.Value);
    }
}
