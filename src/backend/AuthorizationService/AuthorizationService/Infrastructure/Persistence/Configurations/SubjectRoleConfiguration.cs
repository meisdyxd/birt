using AuthorizationService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Infrastructure.Persistence.Configurations;

public class SubjectRoleConfiguration : IEntityTypeConfiguration<SubjectRole>
{
    public void Configure(EntityTypeBuilder<SubjectRole> builder)
    {
        throw new NotImplementedException();
    }
}
