using CSharpFunctionalExtensions;
using Shared.ResultPattern;

namespace AuthorizationService.Infrastructure.Persistence.Entities;

/// <summary>
/// Разрешение
/// </summary>
public class Permission
{
    protected Permission(){}

    /// <summary>
    /// Идентификатор разрешения
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Код разрешения
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Имя разрешения
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание разрешения
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Идентификатор типа ресурса
    /// </summary>
    public Guid ResourceTypeId { get; set; }

    public ResourceType ResourceType { get; set; } = null!;

    public ICollection<RolePermission> RolePermissions { get; set; } = [];

    private Permission(
        string code,
        string name,
        string? description,
        Guid resourceTypeId)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Description = description;
        ResourceTypeId = resourceTypeId;
    }

    public static Result<Permission, Error> Create(
        string code,
        string name,
        string? description,
        Guid resourceTypeId)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            return new Error("Code or name the permission is null", "create-permission", "autzh-db");

        if (resourceTypeId == Guid.Empty)
            return new Error("Resource type id is null", "create-permission", "autzh-db");

        return new Permission(code, name, description, resourceTypeId);
    }
}
