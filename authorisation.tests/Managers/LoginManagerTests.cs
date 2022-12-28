using authorisation.api.Classes;
using authorisation.api.DataManagers.Contracts;
using authorisation.api.Entities;
using authorisation.api.Managers;
using authorisation.api.Managers.Contracts;
using authorisation.api.Services;
using authorisation.api.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace authorisation.tests.Managers;

public class LoginManagerTests
{
    [Fact]
    public async void LoginManager_Authenticate_Successful()
    {
    }

    [Theory]
    [InlineData("LOGIN_EMAIL@TEST.COM", "INCORRECT_LOGIN_PASSWORD")]
    [InlineData("INCORRECT_LOGIN_EMAIL@TEST.COM", "LOGIN_PASSWORD")]
    public async void LoginManager_Authenticate_Unsuccessful(string email, string password)
    {
        var loginManager = new Mock<ILoginManager>().Object;

        var login = new LoginModel()
        {
            Email = email,
            Password = password,
        };
        
        var result = await loginManager.Authenticate(login);

        Assert.Null(result);
    }
    
    [Fact]
    public void LoginManager_GenerateToken_Successful()
    {
    }
    
    [Fact]
    public void LoginManager_GenerateToken_Unsuccessful()
    {
    }
}