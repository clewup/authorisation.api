using authorisation.api.Classes;

namespace authorisation.api.Managers.Contracts;

public interface IUserManager
{
    Task<List<UserModel>> GetUsers();
    Task<UserModel?> GetUser(Guid id);
    Task<UserModel> CreateUser(RegisterModel user);
    Task<UserModel> UpdateUser(UserModel user);
    Task<bool> ValidateEmail(RegisterModel user);
    bool ValidatePasswords(RegisterModel user);
}