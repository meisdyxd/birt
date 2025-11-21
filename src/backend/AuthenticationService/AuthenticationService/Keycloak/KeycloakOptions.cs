using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Keycloak;

public sealed record KeycloakOptions
{
    public const string Section = "Keycloak";

    [Required] public string BaseAddress { get; set; } = null!;
    [Required] public long TimeoutMs { get; set; }
    [Required] public string Realm { get; set; } = null!;
    [Required] public string ClientId { get; set; } = null!;
    [Required] public string ClientSecret { get; set; } = null!;
}
