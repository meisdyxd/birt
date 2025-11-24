using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Infrastructure.Persistence.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("resources", act => act.HasComment("Ресурсы"));

        builder.HasKey(r => r.Id)
            .HasName("pk_resource_id");

        builder.Property(r => r.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("Идентификатор")
            .HasColumnName("id");

        builder.Property(r => r.ResourceTypeId)
            .IsRequired()
            .HasComment("Идентификатор типа ресурса")
            .HasColumnName("resource_type_id");

        builder.Property(r => r.ExternalId)
            .IsRequired()
            .HasComment("Внешний доменный идентификатор")
            .HasColumnName("external_id");

        builder.Property(r => r.OwnerSubjectId)
            .IsRequired(false)
            .HasComment("Идентификатор владельца")
            .HasColumnName("owner_subject_id");

        builder.HasOne(r => r.ResourceType)
            .WithMany(rt => rt.Resources)
            .HasForeignKey(r => r.ResourceTypeId);

        builder.HasOne(r => r.OwnerSubject)
            .WithMany(os => os.Resources)
            .HasForeignKey(r => r.OwnerSubjectId);

        builder.HasMany(r => r.SubjectRoles)
            .WithOne(sr => sr.Resource)
            .HasForeignKey(sr => sr.ResourceId);
    }
}
