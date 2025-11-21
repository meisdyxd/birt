using AuthenticationService.Modules.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly CreateUserHandler _handler;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        CreateUserHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var response = await _handler.Handle(request, cancellationToken);
        if (response.IsFailure)
            return BadRequest(response.Error);
        return Ok(response.Value);
    }
}
