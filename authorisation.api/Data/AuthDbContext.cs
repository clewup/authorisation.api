using authorisation.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace authorisation.api.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext() { }
    public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options) {  }
    
    public virtual DbSet<UserEntity> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}