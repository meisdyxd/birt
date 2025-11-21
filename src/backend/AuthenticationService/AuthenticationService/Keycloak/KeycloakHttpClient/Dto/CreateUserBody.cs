namespace AuthenticationService.Keycloak.KeycloakHttpClient.Dto;

public sealed record CreateUserBody
{
    public CreateUserBody(
        string username,
        string email, 
        string firstname,
        string? lastname, 
        string password)
    {
        Username = username;
        Enabled = true;
        Email = email;
        FirstName = firstname;
        LastName = lastname ?? "undefined";
        EmailVerified = false;
        Credentials = [new("password", password, false)];
    }

    public string Username { get; set; }
    public bool Enabled { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool EmailVerified { get; set; }
    public List<KeycloakCredential> Credentials { get; set; } = [];
}
