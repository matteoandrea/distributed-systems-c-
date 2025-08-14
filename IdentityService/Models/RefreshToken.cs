namespace IdentityService.Models;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TokenHash { get; set; } = null!; // SHA256 do token
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public string? ReplacedByHash { get; set; }
    public string CreatedByIp { get; set; } = null!;

    // FK para User
    public Guid UserId { get; set; } = Guid.Empty!;
    public User User { get; set; } = null!;
}