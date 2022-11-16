using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using authorisation.api.Classes;
using authorisation.api.Data;
using authorisation.api.Entities;
using authorisation.api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace authorisation.api.Managers;

public class LoginManager
{
    private readonly IMongoCollection<UserEntity> _user;
    private readonly PasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;

    public LoginManager(IConfiguration configuration, IOptions<DbConfig> config, PasswordHasher passwordHasher)
    {
        _configuration = configuration;

        var mongoClient = new MongoClient(config.Value.ConnectionString);

        _user = mongoClient
            .GetDatabase(config.Value.DatabaseName)
            .GetCollection<UserEntity>(config.Value.UserCollectionName);

        _passwordHasher = passwordHasher;
    }

    public async Task<UserModel?> Authenticate(LoginModel login)
    {
        var user = await _user.Find(u => u.Email == login.Email).FirstOrDefaultAsync();

        if (user != null)
        {
            var isPasswordMatch = _passwordHasher.ValidatePassword(login.Password, user.Password);

            if (isPasswordMatch)
                return user.ToUserModel();
        }

        return null;
    }

    public string GenerateToken(UserModel user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("email", user.Email),
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
