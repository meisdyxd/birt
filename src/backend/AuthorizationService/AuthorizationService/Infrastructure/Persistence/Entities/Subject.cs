namespace AuthorizationService.Infrastructure.Persistence.Entities;

public class Subject
{
    public Guid Id { get; set; }
    public Guid UserExternalId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
}
