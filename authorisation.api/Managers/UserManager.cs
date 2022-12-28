using authorisation.api.Classes;
using authorisation.api.DataManagers.Contracts;
using authorisation.api.Managers.Contracts;
using authorisation.api.Services;
using AutoMapper;

namespace authorisation.api.Managers;

public class UserManager : IUserManager
{
    private readonly IUserDataManager _userDataManager;
    private readonly IMapper _mapper;

    public UserManager(IUserDataManager userDataManager, IMapper mapper)
    {
        _userDataManager = userDataManager;
        _mapper = mapper;
    }

    public async Task<List<UserModel>> GetUsers()
    {
        var users = await _userDataManager.GetUsers();
        return _mapper.Map<List<UserModel>>(users);
    }

    public async Task<UserModel?> GetUser(Guid id)
    {
        var user = await _userDataManager.GetUser(id);
        return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel> CreateUser(RegisterModel user)
    {
        var createdUser = await _userDataManager.CreateUser(user);
        return _mapper.Map<UserModel>(createdUser);
    }

    public async Task<UserModel> UpdateUser(UserModel user)
    {
        var updatedUser = await _userDataManager.UpdateUser(user);
        return _mapper.Map<UserModel>(updatedUser);
    }

    public async Task<bool> ValidateEmail(RegisterModel user)
    {
        var email = await _userDataManager.GetUserByEmail(user.Email);

        return email == null;
    }
        
    public bool ValidatePasswords(RegisterModel user)
    {
        return user.Password == user.ConfirmPassword;
    }
}