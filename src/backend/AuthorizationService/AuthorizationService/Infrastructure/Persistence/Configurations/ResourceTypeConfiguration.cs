using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Infrastructure.Persistence.Configurations;

public class ResourceTypeConfiguration : IEntityTypeConfiguration<ResourceType>
{
    public void Configure(EntityTypeBuilder<ResourceType> builder)
    {
        builder.ToTable("resource_types", act => act.HasComment("Типы ресурсов"));

        builder.HasIndex(rt => rt.Code, "idx_code_resource_types")
            .IsUnique();

        builder.HasKey(rt => rt.Id)
            .HasName("pk_id");

        builder.Property(rt => rt.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("Идентификатор")
            .HasColumnName("id");

        builder.Property(rt => rt.Code)
            .IsRequired()
            .HasComment("Код")
            .HasColumnName("code");

        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasComment("Наименование")
            .HasColumnName("name");

        builder.Property(rt => rt.Description)
            .IsRequired(false)
            .HasComment("Описание")
            .HasColumnName("description");
    }
}
