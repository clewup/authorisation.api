using authorisation.api.Classes;
using authorisation.api.Infrastructure;
using authorisation.api.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authorisation.api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager _userManager;

    public UserController(ILogger<UserController> logger, UserManager userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(Policy = RoleType.Employee)]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _userManager.GetUsers();

            return Ok(users);
        }
        catch (Exception)
        {
            _logger.LogCritical($"UserController.GetUsers: Error retrieving users");
            throw;
        }
    }

    [HttpGet]
    [Route("{userId}")]
    [Authorize(Policy = RoleType.User)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        try
        {
            var user = await _userManager.GetUser(id);

            return Ok(user);
        }
        catch (Exception)
        {
            _logger.LogCritical("UserController.GetUser: Error retrieving user {Id}", id);
            throw;
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterModel user)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var isValidEmail = await _userManager.ValidateEmail(user);
            if (!isValidEmail)
                return Conflict("Email already exists.");
                
            var isValidPasswords = _userManager.ValidatePasswords(user);
            if (!isValidPasswords)
                return Conflict("Passwords do not match.");

            var createdUser = await _userManager.CreateUser(user);

            return Created("User", createdUser);
        }
        catch (Exception)
        {
            _logger.LogCritical("UserController.CreateUser: Error creating user {UserEmail}", user.Email);
            throw;
        }
    }
        
    [HttpPut]
    [Authorize(Policy = RoleType.User)]
    public async Task<IActionResult> UpdateUser([FromBody] UserModel user)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.GetUser(user.Id);
            if (existingUser == null)
                return NoContent();
                
            var updatedUser = await _userManager.UpdateUser(user);

            return Ok(updatedUser);
        }
        catch (Exception)
        {
            _logger.LogCritical("UserController.UpdateUser: Error updating user {UserId}", user.Id);
            throw;
        }
    }
}