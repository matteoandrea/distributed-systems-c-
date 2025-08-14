using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityService.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly byte[] _key;

    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
    }
    public string GenerateAccessToken(User user, IList<string> roles)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.UserName ?? string.Empty),
            new (JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        var creds = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var random = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(random);
        return Convert.ToBase64String(random);
    }

    public string HashToken(string token)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }
}
