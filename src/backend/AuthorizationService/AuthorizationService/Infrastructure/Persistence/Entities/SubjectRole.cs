using CSharpFunctionalExtensions;
using Shared.ResultPattern;

namespace AuthorizationService.Infrastructure.Persistence.Entities;

public class SubjectRole
{
    protected SubjectRole(){}

    /// <summary>
    /// Внутренний идентификатор пользователя
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// Идентификатор роли
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Идентификатор ресурса
    /// </summary>
    public Guid? ResourceId { get; set; }

    /// <summary>
    /// Дата выдачи роли
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// Дата деактивирования роли
    /// </summary>
    public DateTime? ValidTo { get; set; }

    public Subject Subject { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public Resource? Resource { get; set; }

    private SubjectRole(
        Guid subjectId,
        Guid roleId,
        Guid? resourceId,
        DateTime? validFrom = null,
        DateTime? validTo = null)
    {
        SubjectId = subjectId;
        RoleId = roleId;
        ResourceId = resourceId;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public static Result<SubjectRole, Error> Create(
        Guid subjectId,
        Guid roleId,
        Guid? resourceId,
        DateTime? validFrom = null,
        DateTime? validTo = null)
    {
        if (subjectId == Guid.Empty)
            return new Error("Subject id is null", "create-subject-role", "autzh-db");

        if (roleId == Guid.Empty)
            return new Error("Role id is null", "create-subject-role", "autzh-db");

        if (resourceId is not null && resourceId == Guid.Empty)
            return new Error("Resource id is null", "create-subject-role", "autzh-db");

        if (validFrom is not null && validFrom == default)
            return new Error($"Valid from is {validFrom}", "create-subject-role", "autzh-db");

        if (validTo is not null && validTo == default)
            return new Error($"Valid to is {validTo}", "create-subject-role", "autzh-db");

        return new SubjectRole(subjectId, roleId, resourceId, validFrom, validTo);
    }
}