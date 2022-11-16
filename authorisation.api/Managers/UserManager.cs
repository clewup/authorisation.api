using authorisation.api.Classes;
using authorisation.api.Data;
using authorisation.api.Entities;
using authorisation.api.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace authorisation.api.Managers;

public class UserManager
{
    private readonly IMongoCollection<User> _user;
        private readonly PasswordHasher _passwordHasher;

        /* User entity contains the hashed password. */

        public UserManager(IOptions<DbConfig> config, PasswordHasher passwordHasher)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);

            _user = mongoClient
                .GetDatabase(config.Value.DatabaseName)
                .GetCollection<User>(config.Value.UserCollectionName);

            _passwordHasher = passwordHasher;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _user.Find(_ => true).ToListAsync();
            return users;
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> RegisterUser(Register user)
        {
            var hashedPassword = _passwordHasher.HashPassword(user.Password);
            var registeredUser = user.Register(hashedPassword);
            await _user.InsertOneAsync(registeredUser);
            return registeredUser;
        }

        public async Task<User> UpdateUser(User user)
        {
            await _user.ReplaceOneAsync(u => u.Id == user.Id, user);
            return user;
        }

        public async Task<bool> ValidateUser(Register user)
        {
            var email = await _user.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
            
            if (email == null)
            {
                if (user.Password == user.ConfirmPassword)
                    return true;
            }

            return false;
        }
}