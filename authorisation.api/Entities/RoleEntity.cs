namespace authorisation.api.Entities;

public class RoleEntity : BaseEntity
{
    public string Name { get; set; } = "";
    
    public ICollection<UserEntity> Users { get; set; }
    public List<UserRoleEntity> UserRoles { get; set; }

}