using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Infrastructure.Persistence;

public class AuthzDbContext(DbContextOptions<AuthzDbContext> options) : DbContext(options)
{
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<ResourceType> ResourceTypes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectRole> SubjectRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthzDbContext).Assembly);
    }
}
