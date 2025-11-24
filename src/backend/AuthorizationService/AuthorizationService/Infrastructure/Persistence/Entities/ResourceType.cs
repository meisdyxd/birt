using CSharpFunctionalExtensions;
using Shared.ResultPattern;

namespace AuthorizationService.Infrastructure.Persistence.Entities;

public class ResourceType
{
    protected ResourceType() { }

    /// <summary>
    /// Идентификатор типа ресурса
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Код типа ресурса
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Наименование типа ресурса
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание типа ресурса
    /// </summary>
    public string? Description { get; set; }

    public ICollection<Resource> Resources { get; set; } = [];
    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<Role> Roles { get; set; } = [];

    private ResourceType(
        string code,
        string name,
        string? description)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Description = description;
    }

    public static Result<ResourceType, Error> Create(
        string code,
        string name,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            return new Error("Code or name the permission is null", "create-resource-type", "autzh-db");
        
        return new ResourceType(code, name, description);
    }
}
