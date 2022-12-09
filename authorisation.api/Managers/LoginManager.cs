using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using authorisation.api.Classes;
using authorisation.api.Services;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace authorisation.api.Managers;

public class LoginManager
{
    private readonly PasswordHasher _passwordHasher;
    private readonly UserDataManager _userDataManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public LoginManager(IConfiguration configuration, UserDataManager userDataManager, IMapper mapper, PasswordHasher passwordHasher)
    {
        _configuration = configuration;
        _userDataManager = userDataManager;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserModel?> Authenticate(LoginModel login)
    {
        var user = await _userDataManager.GetUserByEmail(login.Email);

        if (user == null)
            return null;
        
        var isPasswordMatch = _passwordHasher.ValidatePassword(login.Password, user.Password);

        return isPasswordMatch ? _mapper.Map<UserModel>(user) : null;
    }

    public string GenerateToken(UserModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = JsonSerializer.Serialize(user.Roles);

        var claims = new ClaimsIdentity(new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("email", user.Email.ToString()),
            new Claim("roles", roles)
        });
        
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Audience = _configuration["Jwt:Audience"],
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = credentials,
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
