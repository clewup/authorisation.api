using authorisation.api.Classes;
using authorisation.api.DataManagers.Contracts;
using authorisation.api.Entities;
using authorisation.api.Infrastructure;
using authorisation.api.Managers;
using authorisation.api.Services.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace authorisation.tests.Managers;

public class UserManagerTests
{
    [Fact]
    public async void UserManager_GetUsers_Successful()
    {
        var users = new List<UserEntity>()
        {
            new UserEntity()
            {
                Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
                FirstName = "USER_1_FIRST_NAME",
                LastName = "USER_1_LAST_NAME",
                Email = "USER_1_EMAIL",
                Password = "USER_1_PASSWORD",
                Role = RoleType.User,
                LineOne = "USER_1_LINE_ONE",
                LineTwo = "USER_1_LINE_TWO",
                LineThree = "USER_1_LINE_THREE",
                Postcode = "USER_1_POSTCODE",
                City = "USER_1_CITY",
                County = "USER_1_COUNTY",
                Country = "USER_1_COUNTRY",
            },
            new UserEntity()
            {
                Id = Guid.Parse("93596784-9668-40A2-A348-67523F4D93D6"),
                FirstName = "USER_2_FIRST_NAME",
                LastName = "USER_2_LAST_NAME",
                Email = "USER_2_EMAIL",
                Password = "USER_2_PASSWORD",
                Role = RoleType.User,
                LineOne = "USER_2_LINE_ONE",
                LineTwo = "USER_2_LINE_TWO",
                LineThree = "USER_2_LINE_THREE",
                Postcode = "USER_2_POSTCODE",
                City = "USER_2_CITY",
                County = "USER_2_COUNTY",
                Country = "USER_2_COUNTRY",
            }
        };
        
        var mappedUsers = new List<UserModel>()
        {
            new UserModel()
            {
                Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
                FirstName = "USER_1_FIRST_NAME",
                LastName = "USER_1_LAST_NAME",
                Email = "USER_1_EMAIL",
                Role = RoleType.User,
                LineOne = "USER_1_LINE_ONE",
                LineTwo = "USER_1_LINE_TWO",
                LineThree = "USER_1_LINE_THREE",
                Postcode = "USER_1_POSTCODE",
                City = "USER_1_CITY",
                County = "USER_1_COUNTY",
                Country = "USER_1_COUNTRY",
            },
            new UserModel()
            {
                Id = Guid.Parse("93596784-9668-40A2-A348-67523F4D93D6"),
                FirstName = "USER_2_FIRST_NAME",
                LastName = "USER_2_LAST_NAME",
                Email = "USER_2_EMAIL",
                Role = RoleType.User,
                LineOne = "USER_2_LINE_ONE",
                LineTwo = "USER_2_LINE_TWO",
                LineThree = "USER_2_LINE_THREE",
                Postcode = "USER_2_POSTCODE",
                City = "USER_2_CITY",
                County = "USER_2_COUNTY",
                Country = "USER_2_COUNTRY",
            }
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        mockedUserDataManager.Setup(x => x.GetUsers()).ReturnsAsync(users);
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(x => x.Map<List<UserModel>>(users)).Returns(mappedUsers);
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var userManager =
            new UserManager(mockedUserDataManager.Object, mockedMapper.Object, mockedPasswordHasher.Object);

        var result = await userManager.GetUsers();
        
        Assert.IsType<List<UserModel>>(result);
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async void UserManager_GetUser_Successful()
    {
        var user = new UserEntity()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            Role = RoleType.User,
            LineOne = "USER_LINE_ONE",
            LineTwo = "USER_LINE_TWO",
            LineThree = "USER_LINE_THREE",
            Postcode = "USER_POSTCODE",
            City = "USER_CITY",
            County = "USER_COUNTY",
            Country = "USER_COUNTRY",
        };
        var mappedUser = new UserModel()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Role = RoleType.User,
            LineOne = "USER_LINE_ONE",
            LineTwo = "USER_LINE_TWO",
            LineThree = "USER_LINE_THREE",
            Postcode = "USER_POSTCODE",
            City = "USER_CITY",
            County = "USER_COUNTY",
            Country = "USER_COUNTRY",
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        mockedUserDataManager.Setup(x => x.GetUser(Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"))).ReturnsAsync(user);
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(x => x.Map<UserModel>(user)).Returns(mappedUser);
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var userManager =
            new UserManager(mockedUserDataManager.Object, mockedMapper.Object, mockedPasswordHasher.Object);

        var result = await userManager.GetUser(Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"));

        Assert.IsType<UserModel>(result);
        Assert.Equal("USER_FIRST_NAME", result?.FirstName);
        Assert.Equal("USER_LAST_NAME", result?.LastName);
        Assert.Equal("USER_EMAIL", result?.Email);
        Assert.Equal(RoleType.User, result?.Role);
        Assert.Equal("USER_LINE_ONE", result?.LineOne);
        Assert.Equal("USER_LINE_TWO", result?.LineTwo);
        Assert.Equal("USER_LINE_THREE", result?.LineThree);
        Assert.Equal("USER_POSTCODE", result?.Postcode);
        Assert.Equal("USER_CITY", result?.City);
        Assert.Equal("USER_COUNTY", result?.County);
        Assert.Equal("USER_COUNTRY", result?.Country);
    }
    
    [Fact]
    public async void UserManager_CreateUser_Successful()
    {
        var register = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "USER_PASSWORD",
        };
        var user = new UserEntity()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            Role = RoleType.User,
            LineOne = "USER_LINE_ONE",
            LineTwo = "USER_LINE_TWO",
            LineThree = "USER_LINE_THREE",
            Postcode = "USER_POSTCODE",
            City = "USER_CITY",
            County = "USER_COUNTY",
            Country = "USER_COUNTRY",
        };
        var mappedUser = new UserModel()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Role = RoleType.User,
            LineOne = "USER_LINE_ONE",
            LineTwo = "USER_LINE_TWO",
            LineThree = "USER_LINE_THREE",
            Postcode = "USER_POSTCODE",
            City = "USER_CITY",
            County = "USER_COUNTY",
            Country = "USER_COUNTRY",
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        mockedUserDataManager.Setup(x => x.CreateUser(register)).ReturnsAsync(user);
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(x => x.Map<UserModel>(user)).Returns(mappedUser);
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var userManager =
            new UserManager(mockedUserDataManager.Object, mockedMapper.Object, mockedPasswordHasher.Object);

        var result = await userManager.CreateUser(register);

        Assert.IsType<UserModel>(result);
        Assert.Equal("USER_FIRST_NAME", result?.FirstName);
        Assert.Equal("USER_LAST_NAME", result?.LastName);
        Assert.Equal("USER_EMAIL", result?.Email);
        Assert.Equal(RoleType.User, result?.Role);
        Assert.Equal("USER_LINE_ONE", result?.LineOne);
        Assert.Equal("USER_LINE_TWO", result?.LineTwo);
        Assert.Equal("USER_LINE_THREE", result?.LineThree);
        Assert.Equal("USER_POSTCODE", result?.Postcode);
        Assert.Equal("USER_CITY", result?.City);
        Assert.Equal("USER_COUNTY", result?.County);
        Assert.Equal("USER_COUNTRY", result?.Country);
    }
    
    [Fact]
    public async void UserManager_UpdateUser_Successful()
    {
        var updatedUser = new UserModel()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME_UPDATED",
            LastName = "USER_LAST_NAME_UPDATED",
            Email = "USER_EMAIL_UPDATED",
            Role = RoleType.Developer,
            LineOne = "USER_LINE_ONE_UPDATED",
            LineTwo = "USER_LINE_TWO_UPDATED",
            LineThree = "USER_LINE_THREE_UPDATED",
            Postcode = "USER_POSTCODE_UPDATED",
            City = "USER_CITY_UPDATED",
            County = "USER_COUNTY_UPDATED",
            Country = "USER_COUNTRY_UPDATED",
        };
        var user = new UserEntity()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME_UPDATED",
            LastName = "USER_LAST_NAME_UPDATED",
            Email = "USER_EMAIL_UPDATED",
            Password = "USER_PASSWORD_UPDATED",
            Role = RoleType.Developer,
            LineOne = "USER_LINE_ONE_UPDATED",
            LineTwo = "USER_LINE_TWO_UPDATED",
            LineThree = "USER_LINE_THREE_UPDATED",
            Postcode = "USER_POSTCODE_UPDATED",
            City = "USER_CITY_UPDATED",
            County = "USER_COUNTY_UPDATED",
            Country = "USER_COUNTRY_UPDATED",
        };
        var mappedUser = new UserModel()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME_UPDATED",
            LastName = "USER_LAST_NAME_UPDATED",
            Email = "USER_EMAIL_UPDATED",
            Role = RoleType.Developer,
            LineOne = "USER_LINE_ONE_UPDATED",
            LineTwo = "USER_LINE_TWO_UPDATED",
            LineThree = "USER_LINE_THREE_UPDATED",
            Postcode = "USER_POSTCODE_UPDATED",
            City = "USER_CITY_UPDATED",
            County = "USER_COUNTY_UPDATED",
            Country = "USER_COUNTRY_UPDATED",
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        mockedUserDataManager.Setup(x => x.UpdateUser(updatedUser)).ReturnsAsync(user);
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(x => x.Map<UserModel>(user)).Returns(mappedUser);
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var userManager =
            new UserManager(mockedUserDataManager.Object, mockedMapper.Object, mockedPasswordHasher.Object);

        var result = await userManager.UpdateUser(updatedUser);

        Assert.IsType<UserModel>(result);
        Assert.Equal("USER_FIRST_NAME_UPDATED", result?.FirstName);
        Assert.Equal("USER_LAST_NAME_UPDATED", result?.LastName);
        Assert.Equal("USER_EMAIL_UPDATED", result?.Email);
        Assert.Equal(RoleType.Developer, result?.Role);
        Assert.Equal("USER_LINE_ONE_UPDATED", result?.LineOne);
        Assert.Equal("USER_LINE_TWO_UPDATED", result?.LineTwo);
        Assert.Equal("USER_LINE_THREE_UPDATED", result?.LineThree);
        Assert.Equal("USER_POSTCODE_UPDATED", result?.Postcode);
        Assert.Equal("USER_CITY_UPDATED", result?.City);
        Assert.Equal("USER_COUNTY_UPDATED", result?.County);
        Assert.Equal("USER_COUNTRY_UPDATED", result?.Country);
    }
    
    [Fact]
    public async void UserManager_ValidateEmail_Successful()
    {
        var register = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "USER_PASSWORD",
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var userManager =
            new UserManager(mockedUserDataManager.Object, mockedMapper.Object, mockedPasswordHasher.Object);

        var result = await userManager.ValidateEmail(register);

        Assert.True(result);
    }
    
    [Fact]
    public async void UserManager_ValidateEmail_Unsuccessful()
    {
        var register = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "USER_PASSWORD",
        };
        var user = new UserEntity()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME_UPDATED",
            LastName = "USER_LAST_NAME_UPDATED",
            Email = "USER_EMAIL_UPDATED",
            Password = "USER_PASSWORD_UPDATED",
            Role = RoleType.Developer,
            LineOne = "USER_LINE_ONE_UPDATED",
            LineTwo = "USER_LINE_TWO_UPDATED",
            LineThree = "USER_LINE_THREE_UPDATED",
            Postcode = "USER_POSTCODE_UPDATED",
            City = "USER_CITY_UPDATED",
            County = "USER_COUNTY_UPDATED",
            Country = "USER_COUNTRY_UPDATED",
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        mockedUserDataManager.Setup(x => x.GetUserByEmail(register.Email)).ReturnsAsync(user);
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var userManager =
            new UserManager(mockedUserDataManager.Object, mockedMapper.Object, mockedPasswordHasher.Object);

        var result = await userManager.ValidateEmail(register);

        Assert.False(result);
    }
    
    [Fact]
    public void UserManager_ValidatePasswords_Successful()
    {
        var register = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "USER_PASSWORD",
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var userManager =
            new UserManager(mockedUserDataManager.Object, mockedMapper.Object, mockedPasswordHasher.Object);

        var result = userManager.ValidatePasswords(register);
        
        Assert.True(result);
    }
    
    [Fact]
    public void UserManager_ValidatePasswords_Unsuccessful()
    {
        var register = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "INVALID_USER_PASSWORD",
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var userManager =
            new UserManager(mockedUserDataManager.Object, mockedMapper.Object, mockedPasswordHasher.Object);

        var result = userManager.ValidatePasswords(register);
        
        Assert.False(result);
    }
}