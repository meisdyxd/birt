using CSharpFunctionalExtensions;
using Shared.ResultPattern;

namespace AuthorizationService.Infrastructure.Persistence.Entities;

/// <summary>
/// Ресурс
/// </summary>
public class Resource
{
    protected Resource() { }

    /// <summary>
    /// Идентификатор ресурса
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор типа ресурса
    /// </summary>
    public Guid ResourceTypeId { get; set; }

    /// <summary>
    /// Внешний идентификатор в доменной системе(стример, платформа и т.д.)
    /// </summary>
    public Guid ExternalId { get; set; }

    /// <summary>
    /// Идентификатор владельца
    /// </summary>
    public Guid? OwnerSubjectId { get; set; }

    // navigations
    public ResourceType ResourceType { get; set; } = null!;
    public Subject? OwnerSubject { get; set; }
    public ICollection<SubjectRole> SubjectRoles { get; set; } = [];

    private Resource(
        Guid resourceTypeId,
        Guid externalId,
        Guid? ownerSubjectId)
    {
        Id = Guid.NewGuid();
        ResourceTypeId = resourceTypeId;
        ExternalId = externalId;
        OwnerSubjectId = ownerSubjectId;
    }

    public static Result<Resource, Error> Create(
        Guid resourceTypeId,
        Guid externalId,
        Guid? ownerSubjectId)
    {
        if (resourceTypeId == Guid.Empty)
            return new Error("Resource type id is null", "create-resource", "autzh-db");

        if (externalId == Guid.Empty)
            return new Error("External id is null", "create-resource", "autzh-db");

        if (ownerSubjectId is not null && ownerSubjectId == Guid.Empty)
            return new Error("Owner subject id is null", "create-resource", "autzh-db");

        return new Resource(resourceTypeId, externalId, ownerSubjectId);
    }
}
