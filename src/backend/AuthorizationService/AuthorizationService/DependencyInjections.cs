using AuthorizationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;

namespace AuthorizationService;

public static class DependencyInjections
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthzDbContext>(act =>
        {
            var connectionString = configuration.GetConnectionString("Postgres").ThrowIfNullOrWhiteSpace("Authz Postgres");
            act.UseNpgsql(connectionString);
        });

        return services;
    }
}
