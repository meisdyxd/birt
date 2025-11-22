namespace AuthorizationService.Infrastructure.Persistence.Entities;

public class SubjectRole
{
    public Guid SubjectId { get; set; }
    public Guid RoleId { get; set; }
    public Guid? ResourceId { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}