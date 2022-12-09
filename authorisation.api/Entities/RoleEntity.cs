namespace authorisation.api.Entities;

public class RoleEntity : BaseEntity
{
    public string Name { get; set; } = "";
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}