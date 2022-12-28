using authorisation.api.Classes;
using authorisation.api.Managers;
using authorisation.api.Managers.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authorisation.api.Controllers;

[ApiController]
[Route("auth/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly ILoginManager _loginManager;

    public LoginController(ILogger<LoginController> logger, ILoginManager loginManager)
    {
        _logger = logger;
        _loginManager = loginManager;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var user = await _loginManager.Authenticate(login);

        if (user != null)
        {
            var token = _loginManager.GenerateToken(user);
            
            var payload = new LoggedInModel()
            {
                User = user,
                AccessToken = token,
            };
            
            return Ok(payload);
        }
        
        _logger.LogInformation("Incorrect username/password for {LoginEmail}", login.Email);
        return NotFound("Incorrect username or password.");
    }
}