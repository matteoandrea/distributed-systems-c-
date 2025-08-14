using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<Guid>>(opt =>
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}