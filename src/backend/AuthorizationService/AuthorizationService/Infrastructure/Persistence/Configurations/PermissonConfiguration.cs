using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Infrastructure.Persistence.Configurations;

public class PermissonConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions", act => act.HasComment("Разрешения"));

        builder.HasIndex(p => p.Code, "idx_code_permissions")
            .IsUnique();

        builder.HasKey(p => p.Id)
            .HasName("pk_id");

        builder.Property(p => p.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("Идентификатор")
            .HasColumnName("id");

        builder.Property(p => p.Code)
            .IsRequired()
            .HasComment("Код")
            .HasColumnName("code");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasComment("Наименование")
            .HasColumnName("name");

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasComment("Описание")
            .HasColumnName("description");

        builder.Property(p => p.ResourceTypeId)
            .IsRequired()
            .HasComment("Тип ресурса")
            .HasColumnName("resource_type_id");

        builder.HasOne(p => p.ResourceType)
            .WithMany(rt => rt.Permissions)
            .HasForeignKey(p => p.ResourceTypeId);

        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId);
    }
}
