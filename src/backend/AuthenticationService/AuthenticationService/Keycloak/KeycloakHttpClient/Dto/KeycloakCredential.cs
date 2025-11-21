namespace AuthenticationService.Keycloak.KeycloakHttpClient.Dto;

public sealed record KeycloakCredential(string Type, string Value, bool Temporary);
