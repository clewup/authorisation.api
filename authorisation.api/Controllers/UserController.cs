using authorisation.api.Classes;
using authorisation.api.Managers;
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
            catch (Exception e)
            {
                _logger.LogCritical("UserController: GetUser - Error:", e);
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
            catch (Exception e)
            {
                _logger.LogCritical($"UserController: GetUser - Error:", e);
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
                
                var isValidEmail = await _userManager.ValidateEmail(user);
                if (!isValidEmail)
                    return Conflict("Invalid Email.");
                
                var isValidPasswords = await _userManager.ValidatePasswords(user);
                if (!isValidPasswords)
                    return Conflict("Passwords do not match.");

                var createdUser = await _userManager.RegisterUser(user);

                return Created("User", createdUser);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"UserController: CreateUser - Error:", e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingUser = await _userManager.GetUser(user.Id);
                if (existingUser == null)
                    return Conflict("User not found.");
                
                var updatedUser = await _userManager.UpdateUser(user);

                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"UserController: UpdateUser - Error:", e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }