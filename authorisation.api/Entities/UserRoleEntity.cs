namespace authorisation.api.Entities;

public class UserRoleEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    
    public Guid RoleId { get; set; }
    public RoleEntity Role { get; set; }
}