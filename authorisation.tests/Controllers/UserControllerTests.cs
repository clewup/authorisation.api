using authorisation.api.Classes;
using authorisation.api.Controllers;
using authorisation.api.Infrastructure;
using authorisation.api.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace authorisation.tests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async void UserController_GetUsers_Successful()
    {
        var users = new List<UserModel>()
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
        
        var mockedLogger = new Mock<ILogger<UserController>>();
        var mockedUserManager = new Mock<IUserManager>();
        mockedUserManager.Setup(x => x.GetUsers()).ReturnsAsync(users);

        var userController = new UserController(mockedLogger.Object, mockedUserManager.Object);

        var result = await userController.GetUsers();
        
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async void UserController_GetUser_Successful()
    {
        var user = new UserModel()
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
        
        var mockedLogger = new Mock<ILogger<UserController>>();
        var mockedUserManager = new Mock<IUserManager>();
        mockedUserManager.Setup(x => x.GetUser(Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"))).ReturnsAsync(user);

        var userController = new UserController(mockedLogger.Object, mockedUserManager.Object);

        var result = await userController.GetUser(Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"));
        
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async void UserController_RegisterUser_Successful()
    {
        var register = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "USER_PASSWORD",
        };
        var user = new UserModel()
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
        
        var mockedLogger = new Mock<ILogger<UserController>>();
        var mockedUserManager = new Mock<IUserManager>();
        mockedUserManager.Setup(x => x.ValidateEmail(register)).ReturnsAsync(true);
        mockedUserManager.Setup(x => x.ValidatePasswords(register)).Returns(true);
        mockedUserManager.Setup(x => x.CreateUser(register)).ReturnsAsync(user);

        var userController = new UserController(mockedLogger.Object, mockedUserManager.Object);

        var result = await userController.RegisterUser(register);
        
        Assert.IsType<CreatedResult>(result);
    }
    
    [Fact]
    public async void UserController_RegisterUser_Email_Unsuccessful()
    {
        var register = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "USER_PASSWORD",
        };
        
        var mockedLogger = new Mock<ILogger<UserController>>();
        var mockedUserManager = new Mock<IUserManager>();
        mockedUserManager.Setup(x => x.ValidateEmail(register)).ReturnsAsync(false);

        var userController = new UserController(mockedLogger.Object, mockedUserManager.Object);

        var result = await userController.RegisterUser(register);
        
        Assert.IsType<ConflictObjectResult>(result);
    }
    
    [Fact]
    public async void UserController_RegisterUser_Passwords_Unsuccessful()
    {
        var register = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "INVALID_USER_PASSWORD",
        };
        
        var mockedLogger = new Mock<ILogger<UserController>>();
        var mockedUserManager = new Mock<IUserManager>();
        mockedUserManager.Setup(x => x.ValidateEmail(register)).ReturnsAsync(true);
        mockedUserManager.Setup(x => x.ValidatePasswords(register)).Returns(false);

        var userController = new UserController(mockedLogger.Object, mockedUserManager.Object);

        var result = await userController.RegisterUser(register);
        
        Assert.IsType<ConflictObjectResult>(result);
    }
    
    [Fact]
    public async void UserController_UpdateUser_Successful()
    {
        var user = new UserModel()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME_UPDATED",
            LastName = "USER_LAST_NAME_UPDATED",
            Email = "USER_EMAIL_UPDATED",
            Role = RoleType.User,
            LineOne = "USER_LINE_ONE_UPDATED",
            LineTwo = "USER_LINE_TWO_UPDATED",
            LineThree = "USER_LINE_THREE_UPDATED",
            Postcode = "USER_POSTCODE_UPDATED",
            City = "USER_CITY_UPDATED",
            County = "USER_COUNTY_UPDATED",
            Country = "USER_COUNTRY_UPDATED",
        };
        
        var mockedLogger = new Mock<ILogger<UserController>>();
        var mockedUserManager = new Mock<IUserManager>();
        mockedUserManager.Setup(x => x.GetUser(Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"))).ReturnsAsync(user);
        mockedUserManager.Setup(x => x.UpdateUser(user)).ReturnsAsync(user);

        var userController = new UserController(mockedLogger.Object, mockedUserManager.Object);

        var result = await userController.UpdateUser(user);
        
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async void UserController_UpdateUser_Unsuccessful()
    {
        var user = new UserModel()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_FIRST_NAME_UPDATED",
            LastName = "USER_LAST_NAME_UPDATED",
            Email = "USER_EMAIL_UPDATED",
            Role = RoleType.User,
            LineOne = "USER_LINE_ONE_UPDATED",
            LineTwo = "USER_LINE_TWO_UPDATED",
            LineThree = "USER_LINE_THREE_UPDATED",
            Postcode = "USER_POSTCODE_UPDATED",
            City = "USER_CITY_UPDATED",
            County = "USER_COUNTY_UPDATED",
            Country = "USER_COUNTRY_UPDATED",
        };
        
        var mockedLogger = new Mock<ILogger<UserController>>();
        var mockedUserManager = new Mock<IUserManager>();

        var userController = new UserController(mockedLogger.Object, mockedUserManager.Object);

        var result = await userController.UpdateUser(user);
        
        Assert.IsType<NoContentResult>(result);
    }
}