using IdentityService.Data;
using IdentityService.Dtos;
using IdentityService.Handlers.Abstractions;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Handlers;

public class RevokeHandler(
    RevokeRequest req,
    AppDbContext db,
    ITokenService tokenService) : IHandler
{
    public async Task<IResult> HandleAsync()
    {
        var hash = tokenService.HashToken(req.RefreshToken);
        var existing = await db.RefreshTokens
            .FirstOrDefaultAsync(r => r.TokenHash == hash);

        if (existing == null) return Results.NotFound();

        existing.Revoked = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return Results.Ok();
    }
}