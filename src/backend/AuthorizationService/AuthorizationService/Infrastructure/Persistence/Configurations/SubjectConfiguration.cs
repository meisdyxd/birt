using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Infrastructure.Persistence.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("subjects", act => act.HasComment("Субъекты(пользователи)"));

        builder.HasIndex(s => s.UserExternalId, "idx_user_external_id_subjects")
            .IsUnique();

        builder.HasKey(s => s.Id)
            .HasName("pk_id");

        builder.Property(s => s.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("Идентификатор")
            .HasColumnName("id");

        builder.Property(s => s.UserExternalId)
            .IsRequired()
            .HasComment("Внешний идентификатор пользователя")
            .HasColumnName("user_external_id");

        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired()
            .HasComment("Дата создания")
            .HasColumnName("created_at");

        builder.Property(s => s.LastUpdatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired()
            .HasComment("Дата последнего обновления")
            .HasColumnName("last_updated_at");

        builder.HasMany(s => s.SubjectRoles)
            .WithOne(sr => sr.Subject)
            .HasForeignKey(sr => sr.SubjectId);
    }
}
