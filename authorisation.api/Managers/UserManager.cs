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
    private readonly IMongoCollection<UserEntity> _user;
        private readonly PasswordHasher _passwordHasher;

        /* UserEntity contains the hashed password.
        UserModel does not contain the hashed password.
        The Ui receives the UserModel. */
        
        public UserManager(IOptions<DbConfig> config, PasswordHasher passwordHasher)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);

            _user = mongoClient
                .GetDatabase(config.Value.DatabaseName)
                .GetCollection<UserEntity>(config.Value.UserCollectionName);

            _passwordHasher = passwordHasher;
        }

        public async Task<List<UserModel>> GetUsers()
        {
            var users = await _user.Find(_ => true).ToListAsync();
            
            return users.ToUserModel();
        }

        public async Task<UserModel> GetUser(Guid id)
        {
            var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();
            return user.ToUserModel();
        }

        public async Task<UserModel> RegisterUser(RegisterModel user)
        {
            var hashedPassword = _passwordHasher.HashPassword(user.Password);
            var registeredUser = user.Register(hashedPassword);
            
            await _user.InsertOneAsync(registeredUser);
            
            return registeredUser.ToUserModel();
        }

        public async Task<bool> ValidateUser(RegisterModel user)
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