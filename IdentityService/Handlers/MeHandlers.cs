using System.Security.Claims;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Models;

namespace IdentityService.Handlers;

public class MeHandler(ClaimsPrincipal user, UserManager<User> userManager) : IHandler
{
    public async Task<IResult> HandleAsync()
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id == null) return Results.InternalServerError();

        var u = await userManager.FindByIdAsync(id);
        return Results.Ok(new
        {
            u?.Id,
            u?.UserName,
            u?.Email,
            u?.FullName
        });
    }
}