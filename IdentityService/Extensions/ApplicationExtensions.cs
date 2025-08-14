using IdentityService.Services;

namespace IdentityService.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddScoped<ITokenService, TokenService>();
        services.AddOpenApi();

        return services;
    }
}