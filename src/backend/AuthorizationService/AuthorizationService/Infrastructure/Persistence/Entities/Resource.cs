namespace AuthorizationService.Infrastructure.Persistence.Entities;

public class Resource
{
    public Guid Id { get; set; }
    public Guid ResourceTypeId { get; set; }
    public Guid ExternalId { get; set; }
    public Guid? OwnerUserId { get; set; }
}
