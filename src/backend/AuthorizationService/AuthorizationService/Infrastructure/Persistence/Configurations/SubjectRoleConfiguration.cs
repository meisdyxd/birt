using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Infrastructure.Persistence.Configurations;

public class SubjectRoleConfiguration : IEntityTypeConfiguration<SubjectRole>
{
    public void Configure(EntityTypeBuilder<SubjectRole> builder)
    {
        builder.ToTable("subject_roles", act => act.HasComment("Связь субъекта с ролями"));

        builder.HasKey(sr => new {sr.SubjectId, sr.RoleId, sr.ResourceId})
            .HasName("pk_subject_id_role_id_resource_id");

        builder.Property(sr => sr.SubjectId)
            .IsRequired()
            .HasComment("Идентификатор субъекта(пользователя)")
            .HasColumnName("subject_id");

        builder.Property(sr => sr.RoleId)
            .IsRequired()
            .HasComment("Идентификатор роли")
            .HasColumnName("role_id");

        builder.Property(sr => sr.ResourceId)
            .IsRequired(false)
            .HasComment("Идентификатор ресурса")
            .HasColumnName("resource_id");

        builder.Property(s => s.ValidFrom)
            .HasDefaultValueSql("now()")
            .IsRequired(false)
            .HasComment("Дата выдачи роли")
            .HasColumnName("valid_from");

        builder.Property(s => s.ValidTo)
            .IsRequired(false)
            .HasComment("Дата деактивирования роли")
            .HasColumnName("valid_to");
    }
}
