namespace AuthorizationService.Infrastructure.Persistence.Entities;

public class ResourceType
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
