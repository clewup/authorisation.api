namespace authorisation.api.Services.Contracts;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool ValidatePassword(string password, string hashedPassword);
}