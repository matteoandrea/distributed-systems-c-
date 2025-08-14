using IdentityService.Data;
using IdentityService.Dtos;
using IdentityService.Handlers.Abstractions;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Handlers;

public class RefreshHandler(
    RefreshRequest req,
    UserManager<User> userManager,
    AppDbContext db,
    ITokenService tokenService) : IHandler
{
    public async Task<IResult> HandleAsync()
    {
        var incomingHash = tokenService.HashToken(req.RefreshToken);
        var existing = await db.RefreshTokens
            .FirstOrDefaultAsync(r => r.TokenHash == incomingHash);

        if (existing == null || existing.Revoked != null || existing.Expires < DateTime.UtcNow)
            return Results.Unauthorized();

        // Encontrar usuário
        var user = await userManager.FindByIdAsync(existing.UserId.ToString());
        if (user == null) return Results.Unauthorized();

        // Rotacionar token: revogar o antigo e criar um novo
        existing.Revoked = DateTime.UtcNow;
        var newRefresh = tokenService.GenerateRefreshToken();
        existing.ReplacedByHash = tokenService.HashToken(newRefresh);

        var newEntity = new RefreshToken
        {
            TokenHash = tokenService.HashToken(newRefresh),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = "127.0.0.1", // TODO: Obter IP real da requisição
            UserId = user.Id
        };

        db.RefreshTokens.Add(newEntity);
        await db.SaveChangesAsync();

        var roles = (await userManager.GetRolesAsync(user)).ToList();
        var newAccess = tokenService.GenerateAccessToken(user, roles);

        return Results.Ok(new TokenResponse(newAccess, newRefresh));
    }
}