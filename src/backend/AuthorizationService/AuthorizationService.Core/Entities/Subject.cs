using CSharpFunctionalExtensions;
using Shared.ResultPattern;

namespace AuthorizationService.Infrastructure.Persistence.Entities;

/// <summary>
/// Субъект(пользователь)
/// </summary>
public class Subject
{
    protected Subject() { }

    /// <summary>
    /// Внутренний идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Внешний идентификатор пользователя
    /// </summary>
    public Guid UserExternalId { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime LastUpdatedAt { get; set; }

    // navigations
    public ICollection<Resource> Resources { get; set; } = [];
    public ICollection<SubjectRole> SubjectRoles { get; set; } = [];

    private Subject(
        Guid userExternalid)
    {
        Id = Guid.NewGuid();
        UserExternalId = userExternalid;
        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public static Result<Subject, Error> Create(Guid userExternalId)
    {
        if (userExternalId == Guid.Empty)
            return new Error("User external id is null", "create-subject", "autzh-db");

        return new Subject(userExternalId);
    }
}
