using authorisation.api.Classes;
using authorisation.api.Entities;
using AutoMapper;

namespace authorisation.api.Services.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<RoleEntity, RoleModel>().ReverseMap();
    }
}