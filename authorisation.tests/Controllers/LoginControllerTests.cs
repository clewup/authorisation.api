using authorisation.api.Classes;
using authorisation.api.Controllers;
using authorisation.api.Infrastructure;
using authorisation.api.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace authorisation.tests.Controllers;

public class LoginControllerTests
{
    [Fact]
    public async void LoginController_Login_Successful()
    {
        var login = new LoginModel()
        {
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
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
        
        var mockedLogger = new Mock<ILogger<LoginController>>();
        var mockedLoginManager = new Mock<ILoginManager>();
        mockedLoginManager.Setup(x => x.Authenticate(login)).ReturnsAsync(user);
        mockedLoginManager.Setup(x => x.GenerateToken(user)).Returns("JWT_TOKEN");

        var loginController = new LoginController(mockedLogger.Object, mockedLoginManager.Object);

        var result = await loginController.Login(login);

        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async void LoginController_Login_Unsuccessful()
    {
        var login = new LoginModel()
        {
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
        };
        
        var mockedLogger = new Mock<ILogger<LoginController>>();
        var mockedLoginManager = new Mock<ILoginManager>();

        var loginController = new LoginController(mockedLogger.Object, mockedLoginManager.Object);

        var result = await loginController.Login(login);

        Assert.IsType<NotFoundObjectResult>(result);
    }
}