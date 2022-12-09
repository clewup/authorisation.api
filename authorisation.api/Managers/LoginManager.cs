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
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>();
        claims.Add(new Claim("userId", user.Id.ToString()));
        claims.Add(new Claim("email", user.Email));

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
