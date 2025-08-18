using IdentityService.Data;
using IdentityService.Dtos;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Shared.Models;

namespace IdentityService.Handlers;

public class LoginHandler(
    LoginRequest req,
    UserManager<User> userManager,
    AppDbContext db,
    ITokenService tokenService) : IHandler
{
    public async Task<IResult> HandleAsync()
    {
        User? user = await userManager.FindByNameAsync(req.Username);
        if (user is null) return Results.Unauthorized();

        if (!await userManager.CheckPasswordAsync(user, req.Password))
            return Results.Unauthorized();

        var roles = (await userManager.GetRolesAsync(user)).ToList();
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken();
        var refreshHash = tokenService.HashToken(refreshToken);

        var rtEntity = new RefreshToken
        {
            TokenHash = refreshHash,
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = "127.0.0.1",
            UserId = user.Id
        };

        db.RefreshTokens.Add(rtEntity);
        await db.SaveChangesAsync();

        return Results.Ok(new TokenResponse(accessToken, refreshToken));
    }
}