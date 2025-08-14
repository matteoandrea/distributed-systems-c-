using IdentityService.Dtos;
using IdentityService.Handlers.Abstractions;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Handlers;

public class RegisterHandler(RegisterRequest req, UserManager<User> userManager) : IHandler
{
    public async Task<IResult> HandleAsync()
    {
        var user = new User
        {
            UserName = req.Username,
            Email = req.Email,
            FullName = req.FullName
        };

        IdentityResult res = await userManager.CreateAsync(user, req.Password);
        if (!res.Succeeded)
            return Results.BadRequest(res.Errors);

        await userManager.AddToRoleAsync(user, "User");
        return Results.Created($"/users/{user.Id}", new
        {
            user.Id,
            user.UserName,
            user.Email
        });
    }
}