using authorisation.api.Services.Contracts;

namespace authorisation.api.Services;

public class PasswordHasher : IPasswordHasher
{
    public PasswordHasher()
    {

    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool ValidatePassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}