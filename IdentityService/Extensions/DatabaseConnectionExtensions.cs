using IdentityService.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Extensions;

public static class DatabaseConnectionExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPooledDbContextFactory<AppDbContext>(opt =>
            opt.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}