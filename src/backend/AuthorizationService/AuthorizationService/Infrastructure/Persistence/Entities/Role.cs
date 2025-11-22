namespace AuthorizationService.Infrastructure.Persistence.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid ResourceTypeId { get; set; }
    public int Level { get; set; }
}
