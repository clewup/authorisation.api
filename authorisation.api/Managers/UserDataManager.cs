using authorisation.api.Classes;
using authorisation.api.Data;
using authorisation.api.Entities;
using authorisation.api.Services;
using Microsoft.EntityFrameworkCore;

namespace authorisation.api.Managers;

public class UserDataManager
{
    private readonly AuthDbContext _context;
    private readonly PasswordHasher _passwordHasher;

    public UserDataManager(AuthDbContext context, PasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<List<UserEntity>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }

    public async Task<UserEntity?> GetUser(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<UserEntity> CreateUser(RegisterModel user)
    {
        var hashedPassword = _passwordHasher.HashPassword(user.Password);
        var defaultRoles = await _context.Roles.Where(r => r.Name == "User").ToListAsync();
        
        var createdUser = new UserEntity()
        {
            Id = new Guid(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = hashedPassword,
            LineOne = "",
            LineTwo = "",
            LineThree = "",
            Postcode = "",
            City = "",
            County = "",
            Country = "",
            Roles = defaultRoles,
        };

        _context.Users.Add(createdUser);
        await _context.SaveChangesAsync();

        return createdUser;
    }

    public async Task<UserEntity> UpdateUser(UserModel user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.LineOne = user.LineOne;
        existingUser.LineTwo = user.LineTwo;
        existingUser.LineThree = user.LineThree;
        existingUser.Postcode = user.Postcode;
        existingUser.City = user.City;
        existingUser.County = user.County;
        existingUser.Country = user.Country;

        await _context.SaveChangesAsync();

        return existingUser;
    }
}