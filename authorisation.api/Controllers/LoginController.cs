using Microsoft.AspNetCore.Mvc;

namespace authorisation.api.Controllers;

[ApiController]
[Route("auth/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Login()
    {
        return Ok();
    }
}