using Microsoft.EntityFrameworkCore;
using BackendApi.Models;

namespace BackendApi.Data
{
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });
        }
    }
}