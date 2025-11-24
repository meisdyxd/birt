using CSharpFunctionalExtensions;
using Shared.ResultPattern;

namespace AuthorizationService.Infrastructure.Persistence.Entities;

public class RolePermission
{
    protected RolePermission() { }

    /// <summary>
    /// Идентификатор роли
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Идентификатор разрешения
    /// </summary>
    public Guid PermissionId { get; set; }

    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;

    private RolePermission(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public static Result<RolePermission, Error> Create(
        Guid roleId,
        Guid permissionId)
    {
        if (roleId == Guid.Empty)
            return new Error("Role id is null", "create-role-permission", "autzh-db");

        if (permissionId == Guid.Empty)
            return new Error("Permission id is null", "create-role-permission", "autzh-db");

        return new RolePermission(roleId, permissionId);
    }
}
