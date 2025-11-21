namespace AuthenticationService.Keycloak.KeycloakHttpClient.Dto;

public sealed record CreateUserDto(string Username, string Email, string FirstName, string? Lastname, string Password);
