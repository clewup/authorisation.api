using authorisation.api.Classes;
using authorisation.api.Entities;

namespace authorisation.api.Services;

public static class UserService
{
    public static UserModel ToUserModel(this UserEntity user)
    {
        return new UserModel()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            LineOne = user.LineOne,
            LineTwo = user.LineTwo,
            LineThree = user.LineThree,
            Postcode = user.Postcode,
            City = user.City,
            County = user.County,
            Country = user.Country
        };
    }

    public static List<UserModel> ToUserModel(this List<UserEntity> users)
    {
        List<UserModel> modelledUsers = new List<UserModel>();

        foreach (var user in users)
        {
            modelledUsers.Add(user.ToUserModel());
        }

        return modelledUsers;
    }
    
    public static UserEntity ToUserEntity(this UserModel user)
    {
        return new UserEntity()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            LineOne = user.LineOne,
            LineTwo = user.LineTwo,
            LineThree = user.LineThree,
            Postcode = user.Postcode,
            City = user.City,
            County = user.County,
            Country = user.Country
        };
    }
}