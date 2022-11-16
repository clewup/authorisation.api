using authorisation.api.Classes;
using authorisation.api.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authorisation.api.Controllers;

[ApiController]
[Route("auth/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly LoginManager _loginManager;

    public LoginController(ILogger<LoginController> logger, LoginManager loginManager)
    {
        _logger = logger;
        _loginManager = loginManager;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        var user = await _loginManager.Authenticate(login);

        if (user != null)
        {
            var token = _loginManager.GenerateToken(user);
            return Ok(token);
        }

        return NotFound("Incorrect username/password.");
    }
}