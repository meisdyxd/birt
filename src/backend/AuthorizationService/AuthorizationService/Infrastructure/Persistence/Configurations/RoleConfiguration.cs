using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles", act => act.HasComment("Роли"));

        builder.HasIndex(r => r.Code, "idx_code_roles")
            .IsUnique();

        builder.HasKey(r => r.Id)
            .HasName("pk_role_id");

        builder.Property(r => r.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("Идентификатор")
            .HasColumnName("id");

        builder.Property(r => r.Code)
            .IsRequired()
            .HasComment("Код")
            .HasColumnName("code");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasComment("Наименование")
            .HasColumnName("name");

        builder.Property(r => r.Description)
            .IsRequired(false)
            .HasComment("Описание")
            .HasColumnName("description");

        builder.Property(r => r.ResourceTypeId)
            .IsRequired()
            .HasComment("Тип ресурса")
            .HasColumnName("resource_type_id");

        builder.HasOne(r => r.ResourceType)
            .WithMany(rt => rt.Roles)
            .HasForeignKey(r => r.ResourceTypeId);

        builder.HasMany(r => r.SubjectRoles)
            .WithOne(sr => sr.Role)
            .HasForeignKey(sr => sr.RoleId);

        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId);
    }
}
