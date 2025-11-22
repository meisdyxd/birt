using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Infrastructure.Persistence;

public class AuthzDbContext(DbContextOptions<AuthzDbContext> options) : DbContext(options)
{

}
