using IdentityService.Models;

namespace IdentityService.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user, IList<string> roles);
    string GenerateRefreshToken();
    string HashToken(string token);
}