using authorisation.api.Classes;
using authorisation.api.DataManagers.Contracts;
using authorisation.api.Entities;
using authorisation.api.Infrastructure;
using authorisation.api.Managers;
using authorisation.api.Services.Contracts;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;

namespace authorisation.tests.Managers;

public class LoginManagerTests
{
    [Fact]
    public async void LoginManager_Authenticate_Successful()
    {
        var login = new LoginModel()
        {
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
        };
        
        var user = new UserEntity()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "HASHED_PASSWORD",
        };
        
        var mappedUser = new UserModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
        };
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        mockedUserDataManager.Setup(x => x.GetUserByEmail(login.Email)).ReturnsAsync(user);
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(x => x.Map<UserModel>(user)).Returns(mappedUser);
        var mockedPasswordHasher = new Mock<IPasswordHasher>();
        mockedPasswordHasher.Setup(x => x.ValidatePassword(login.Password, user.Password)).Returns(true);
        var mockedConfiguration = new Mock<IConfiguration>();

        var loginManager = new LoginManager(mockedUserDataManager.Object, mockedMapper.Object,
            mockedPasswordHasher.Object, mockedConfiguration.Object);

        var result = await loginManager.Authenticate(login);

        Assert.Equal("USER_FIRST_NAME", result?.FirstName);
        Assert.Equal("USER_LAST_NAME", result?.LastName);
        Assert.Equal("USER_EMAIL", result?.Email);
    }

    [Fact]
    public void LoginManager_GenerateToken_Successful()
    {
        var user = new UserModel()
        {
            Id = Guid.Parse("67FB15CB-A5D2-4036-BC49-BA6BA3298BF7"),
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Role = RoleType.User,
        };
        
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Issuer", "JWT_ISSUER"},
            {"Jwt:Audience", "JWT_AUDIENCE"},
            {"Jwt:Key", "Ju2FrLK375iBvFT0O2aJUMpdgMo1qeSxJS97dCHIlaE"},
        };
        IConfiguration mockedConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        
        var mockedUserDataManager = new Mock<IUserDataManager>();
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        var loginManager = new LoginManager(mockedUserDataManager.Object, mockedMapper.Object,
            mockedPasswordHasher.Object, mockedConfiguration);

        var result = loginManager.GenerateToken(user);
        
        Assert.NotNull(result);
    }
}