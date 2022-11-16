using authorisation.api.Classes;
using authorisation.api.Entities;
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
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userManager.GetUsers();

                return Ok(users);
            }
            catch
            {
                _logger.LogWarning("UserController, GetUser: Error retrieving users.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _userManager.GetUser(id);

                return Ok(user);
            }
            catch
            {
                _logger.LogWarning($"UserController, GetUser: Error retrieving user {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var isValid = await _userManager.ValidateUser(user);
                if (!isValid)
                    return Conflict("User cannot be validated.");

                var createdUser = _userManager.RegisterUser(user).Result;

                return Created("User", createdUser);
            }
            catch
            {
                _logger.LogWarning($"UserController, CreateUser: Error creating user {user.Email}.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }