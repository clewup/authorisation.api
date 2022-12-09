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
            .HasMany(u => u.Roles)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .IsRequired();
    }
}