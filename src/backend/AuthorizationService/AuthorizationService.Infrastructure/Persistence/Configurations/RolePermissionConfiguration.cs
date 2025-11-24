using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions", act => act.HasComment("Связь ролей с разрешениями"));

        builder.HasKey(rp => new {rp.RoleId, rp.PermissionId})
            .HasName("pk_role_id_permission_id");

        builder.Property(rp => rp.RoleId)
            .IsRequired()
            .HasComment("Идентификатор роли")
            .HasColumnName("role_id");

        builder.Property(rp => rp.PermissionId)
            .IsRequired()
            .HasComment("Идентификатор разрешения")
            .HasColumnName("permission_id");
    }
}
