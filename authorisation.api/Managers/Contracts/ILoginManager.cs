using authorisation.api.Classes;

namespace authorisation.api.Managers.Contracts;

public interface ILoginManager
{
    Task<UserModel?> Authenticate(LoginModel login);
    string GenerateToken(UserModel user);
}