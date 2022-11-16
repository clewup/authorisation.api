using authorisation.api.Classes;
using authorisation.api.Entities;

namespace authorisation.api.Services;

public static class Registration
{
    public static User Register(this Register user, string hashedPassword)
    {
        return new User()
        {
            Id = new Guid(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = hashedPassword,
            LineOne = null,
            LineTwo = null,
            LineThree = null,
            Postcode = null,
            City = null,
            County = null,
            Country = null,
        };
    }
}