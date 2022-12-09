using authorisation.api.Classes;
using authorisation.api.Entities;
using AutoMapper;

namespace authorisation.api.Services.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, UserModel>().ReverseMap();
    }
}