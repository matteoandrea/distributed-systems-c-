using IdentityService.Data;
using IdentityService.Dtos;
using IdentityService.Handlers;
using IdentityService.Models;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityService.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        // var authGroup = app.MapGroup("/auth");
        var authGroup = app.MapGroup("");
        authGroup.MapGet("", [Authorize] () =>
        {
            return new { res = "heeee" };
        }).WithOpenApi();

        // Register
        authGroup.MapPost("/register", async (
            [FromBody] RegisterRequest req,
            [FromServices] UserManager<User> userManager)
            => await new RegisterHandler(req, userManager).HandleAsync())
        .WithName("Register")
        .WithOpenApi();

        // Login
        authGroup.MapPost("/login", async (
            [FromBody] LoginRequest req,
            [FromServices] UserManager<User> userManager,
            [FromServices] AppDbContext db,
            [FromServices] ITokenService tokenService)
            => await new LoginHandler(req, userManager, db, tokenService).HandleAsync())
        .WithName("Login")
        .WithOpenApi();

        // Refresh token
        authGroup.MapPost("/refresh", async (
            [FromBody] RefreshRequest req,
            [FromServices] UserManager<User> userManager,
            [FromServices] AppDbContext db,
            [FromServices] ITokenService tokenService)
            => await new RefreshHandler(req, userManager, db, tokenService).HandleAsync())
        .WithName("RefreshToken")
        .WithOpenApi();

        // Revoke token
        authGroup.MapPost("/revoke", async (
            [FromBody] RevokeRequest req,
            [FromServices] AppDbContext db,
            [FromServices] ITokenService tokenService)
            => await new RevokeHandler(req, db, tokenService).HandleAsync())
        .WithName("RevokeToken")
        .WithOpenApi();

        authGroup.MapGet("/me", [Authorize] async (
            ClaimsPrincipal user,
            UserManager<User> userManager)
            => await new MeHandler(user, userManager).HandleAsync())
        .WithName("GetCurrentUser")
        .WithOpenApi();
    }
}