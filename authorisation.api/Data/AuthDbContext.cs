using authorisation.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace authorisation.api.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options) {  }
    
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<RoleEntity> Roles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasMany(c => c.Roles)
            .WithMany(p => p.Users)
            .UsingEntity<UserRoleEntity>(
                j => j
                    .HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId),
                j => j
                    .HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId),
                j =>
                {
                    j.ToTable("UserRoles");
                    j.HasKey(cp => new { cp.UserId, cp.RoleId });
                });
    }
}