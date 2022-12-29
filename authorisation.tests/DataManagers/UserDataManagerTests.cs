using authorisation.api.Classes;
using authorisation.api.Data;
using authorisation.api.DataManagers;
using authorisation.api.DataManagers.Contracts;
using authorisation.api.Entities;
using authorisation.api.Infrastructure;
using authorisation.api.Services.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace authorisation.tests.DataManagers;

public class UserDataManagerTests
{
    DbContextOptions<AuthDbContext> options = new DbContextOptionsBuilder<AuthDbContext>()
        .UseInMemoryDatabase(databaseName: "AuthDb")
        .Options;
    
    public UserDataManagerTests()
    {
        using (var context = new AuthDbContext(options))
        {
            context.Users.Add(new UserEntity()
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
            });
            context.Users.Add(new UserEntity()
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
            });
            context.SaveChangesAsync();
        }
    }
    [Fact]
    public async Task UserDataManager_GetUsers_Successful()
    {
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        using (var context = new AuthDbContext(options))
        {
            var userDataManager = new UserDataManager(context, mockedMapper.Object, mockedPasswordHasher.Object);

            var result = await userDataManager.GetUsers();
            
            Assert.Equal(2, result.Count());
        }
    }
    
    [Fact]
    public async void UserDataManager_GetUser_Successful()
    {
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        using (var context = new AuthDbContext(options))
        {
            var userDataManager = new UserDataManager(context, mockedMapper.Object, mockedPasswordHasher.Object);

            var result = await userDataManager.GetUser(Guid.Parse("93596784-9668-40A2-A348-67523F4D93D6"));
            
            Assert.Equal("USER_2_FIRST_NAME", result?.FirstName);
            Assert.Equal("USER_2_LAST_NAME", result?.LastName);
            Assert.Equal("USER_2_EMAIL", result?.Email);
            Assert.Equal(RoleType.User, result?.Role);
            Assert.Equal("USER_2_LINE_ONE", result?.LineOne);
            Assert.Equal("USER_2_LINE_TWO", result?.LineTwo);
            Assert.Equal("USER_2_LINE_THREE", result?.LineThree);
            Assert.Equal("USER_2_POSTCODE", result?.Postcode);
            Assert.Equal("USER_2_CITY", result?.City);
            Assert.Equal("USER_2_COUNTY", result?.County);
            Assert.Equal("USER_2_COUNTRY", result?.Country);
        }
    }
    
    [Fact]
    public async void UserDataManager_GetUser_Unsuccessful()
    {
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        using (var context = new AuthDbContext(options))
        {
            var userDataManager = new UserDataManager(context, mockedMapper.Object, mockedPasswordHasher.Object);

            var result = await userDataManager.GetUser(Guid.Parse("93596784-9668-40A2-A348-67523F4D93D7"));
            
            Assert.Null(result);
        }
    }
    
    [Fact]
    public async void UserDataManager_GetUserByEmail_Successful()
    {
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        using (var context = new AuthDbContext(options))
        {
            var userDataManager = new UserDataManager(context, mockedMapper.Object, mockedPasswordHasher.Object);

            var result = await userDataManager.GetUserByEmail("USER_2_EMAIL");
            
            Assert.Equal("USER_2_FIRST_NAME", result?.FirstName);
            Assert.Equal("USER_2_LAST_NAME", result?.LastName);
            Assert.Equal("USER_2_EMAIL", result?.Email);
            Assert.Equal(RoleType.User, result?.Role);
            Assert.Equal("USER_2_LINE_ONE", result?.LineOne);
            Assert.Equal("USER_2_LINE_TWO", result?.LineTwo);
            Assert.Equal("USER_2_LINE_THREE", result?.LineThree);
            Assert.Equal("USER_2_POSTCODE", result?.Postcode);
            Assert.Equal("USER_2_CITY", result?.City);
            Assert.Equal("USER_2_COUNTY", result?.County);
            Assert.Equal("USER_2_COUNTRY", result?.Country);
        }
    }
    
    [Fact]
    public async void UserDataManager_GetUserByEmail_Unsuccessful()
    {
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();

        using (var context = new AuthDbContext(options))
        {
            var userDataManager = new UserDataManager(context, mockedMapper.Object, mockedPasswordHasher.Object);

            var result = await userDataManager.GetUserByEmail("USER_2_INVALID_EMAIL");
            
            Assert.Null(result);
        }
    }
    
    [Fact]
    public async void UserDataManager_CreateUser_Successful()
    {
        var user = new RegisterModel()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "USER_PASSWORD",
            ConfirmPassword = "USER_PASSWORD"
        };
        var mappedUser = new UserEntity()
        {
            FirstName = "USER_FIRST_NAME",
            LastName = "USER_LAST_NAME",
            Email = "USER_EMAIL",
            Password = "HASHED_PASSWORD",
        };

        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(x => x.Map<UserEntity>(user)).Returns(mappedUser);
        var mockedPasswordHasher = new Mock<IPasswordHasher>();
        mockedPasswordHasher.Setup(x => x.HashPassword(user.Password)).Returns("HASHED_PASSWORD");

        using (var context = new AuthDbContext(options))
        {
            var userDataManager = new UserDataManager(context, mockedMapper.Object, mockedPasswordHasher.Object);
            
            var result = await userDataManager.CreateUser(user);
            
            Assert.Equal("USER_FIRST_NAME", result?.FirstName);
            Assert.Equal("USER_LAST_NAME", result?.LastName);
            Assert.Equal("USER_EMAIL", result?.Email);
            Assert.Equal(RoleType.User, result?.Role);
        }
    }
    
    [Fact]
    public async void UserDataManager_UpdateUser_Successful()
    {
        var mockedMapper = new Mock<IMapper>();
        var mockedPasswordHasher = new Mock<IPasswordHasher>();
        
        var user = new UserModel()
        {
            Id = Guid.Parse("3E1A122D-DE6D-4934-8815-8D468865C5DC"),
            FirstName = "USER_1_FIRST_NAME_UPDATED",
            LastName = "USER_1_LAST_NAME_UPDATED",
            Email = "USER_1_EMAIL_UPDATED",
            Role = RoleType.Developer,
            LineOne = "USER_1_LINE_ONE_UPDATED",
            LineTwo = "USER_1_LINE_TWO_UPDATED",
            LineThree = "USER_1_LINE_THREE_UPDATED",
            Postcode = "USER_1_POSTCODE_UPDATED",
            City = "USER_1_CITY_UPDATED",
            County = "USER_1_COUNTY_UPDATED",
            Country = "USER_1_COUNTRY_UPDATED",
        };
        
        using (var context = new AuthDbContext(options))
        {
            var userDataManager = new UserDataManager(context, mockedMapper.Object, mockedPasswordHasher.Object);

            var result = await userDataManager.UpdateUser(user);
            
            Assert.Equal("USER_1_FIRST_NAME_UPDATED", result?.FirstName);
            Assert.Equal("USER_1_LAST_NAME_UPDATED", result?.LastName);
            Assert.Equal("USER_1_EMAIL_UPDATED", result?.Email);
            Assert.Equal(RoleType.Developer, result?.Role);
            Assert.Equal("USER_1_LINE_ONE_UPDATED", result?.LineOne);
            Assert.Equal("USER_1_LINE_TWO_UPDATED", result?.LineTwo);
            Assert.Equal("USER_1_LINE_THREE_UPDATED", result?.LineThree);
            Assert.Equal("USER_1_POSTCODE_UPDATED", result?.Postcode);
            Assert.Equal("USER_1_CITY_UPDATED", result?.City);
            Assert.Equal("USER_1_COUNTY_UPDATED", result?.County);
            Assert.Equal("USER_1_COUNTRY_UPDATED", result?.Country);
        }
    }
}