using AuthenticationService.Keycloak;
using AuthenticationService.Keycloak.KeycloakHttpClient;
using AuthenticationService.UseCases.CreateUser;
using Shared.CQRS;
using Shared.Extensions;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;

namespace AuthenticationService;

public static class DependencyInjection
{
    public static IServiceCollection AddKeycloakClient(this IServiceCollection services, IConfiguration configuration)
    {
        var keycloakSection = configuration.GetSection(KeycloakOptions.Section);
        var baseAdrress = keycloakSection.GetValue<string>("BaseAddress").ThrowIfNullOrEmpty("Keycloak BaseAddress");
        var timeout = keycloakSection.GetValue<int>("TimeoutMs").ThrowIfDefault("Keycloak Timeout");

        services.AddOptions<KeycloakOptions>()
            .Bind(keycloakSection)
            .ValidateOnStart()
            .ValidateDataAnnotations();

        services.AddHttpClient<IKeycloakClient, KeycloakClient>("KeycloakClient", cfg =>
        {
            cfg.BaseAddress = new(baseAdrress);
            cfg.Timeout = TimeSpan.FromMilliseconds(timeout);
        });

        return services;
    }

    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.Scan(action =>
        {
            action.FromAssemblyOf<CreateUserHandler>()
                .AddClasses(action => action.AssignableTo(typeof(ICommandHandler<>)))
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
                .AddClasses(action => action.AssignableTo(typeof(ICommandHandler<,>)))
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime();
        });

        return services;
    }

    public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var connectionString = configuration.GetConnectionString("Redis").ThrowIfNullOrEmpty("Redis connection string");

            var options = ConfigurationOptions.Parse(connectionString);
            options.AbortOnConnectFail = false;

            return ConnectionMultiplexer.Connect(options);
        });

        return services;
    }
}
