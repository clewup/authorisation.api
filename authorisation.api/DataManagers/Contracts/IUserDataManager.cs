using authorisation.api.Classes;
using authorisation.api.Entities;

namespace authorisation.api.DataManagers.Contracts;

public interface IUserDataManager
{
    Task<List<UserEntity>> GetUsers();
    Task<UserEntity?> GetUser(Guid id);
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity> CreateUser(RegisterModel user);
    Task<UserEntity?> UpdateUser(UserModel user);
}