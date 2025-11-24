using CSharpFunctionalExtensions;
using Shared.ResultPattern;

namespace AuthorizationService.Infrastructure.Persistence.Entities;

/// <summary>
/// Роль
/// </summary>
public class Role
{
    protected Role() { }

    /// <summary>
    /// Идентификатор роли
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Код роли
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Наименование роли
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание роли
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Идентификатор типа ресурса
    /// </summary>
    public Guid ResourceTypeId { get; set; }

    /// <summary>
    /// Уровень роли
    /// </summary>
    public int Level { get; set; }

    // navigations
    public ResourceType ResourceType { get; set; } = null!;
    public ICollection<RolePermission> RolePermissions { get; set; } = [];
    public ICollection<SubjectRole> SubjectRoles { get; set; } = [];

    private Role(
        string code,
        string name,
        string? description,
        Guid resourceTypeId,
        int level)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Description = description;
        ResourceTypeId = resourceTypeId;
        Level = level;
    }

    public static Result<Role, Error> Create(
        string code,
        string name,
        string? description,
        Guid resourceTypeId,
        int level)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            return new Error("Code or name the permission is null", "create-role", "autzh-db");

        if (resourceTypeId == Guid.Empty)
            return new Error("Resource type id is null", "create-role", "autzh-db");

        return new Role(code, name, description, resourceTypeId, level);
    }
}
