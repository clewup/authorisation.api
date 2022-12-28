using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using authorisation.api.Classes;
using authorisation.api.DataManagers.Contracts;
using authorisation.api.Managers.Contracts;
using authorisation.api.Services.Contracts;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace authorisation.api.Managers;

public class LoginManager : ILoginManager
{
    private readonly IUserDataManager _userDataManager;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;

    public LoginManager(IUserDataManager userDataManager, IMapper mapper, IPasswordHasher passwordHasher, IConfiguration configuration)
    {
        _userDataManager = userDataManager;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
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
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

        if (jwtIssuer == null || jwtAudience == null || jwtKey == null)
        {
            jwtIssuer = _configuration["Jwt:Issuer"];
            jwtAudience = _configuration["Jwt:Audience"];
            jwtKey = _configuration["Jwt:Key"];
        }
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(ClaimTypes.Role, user.Role));
        
        claims.Add(new Claim("id", user.Id.ToString()));
        claims.Add(new Claim("email", user.Email));
        claims.Add(new Claim("role", user.Role));

        var token = new JwtSecurityToken(jwtIssuer,
            jwtAudience,
            claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
