namespace IdentityService.Dtos;

public record RegisterRequest(string Username, string Email, string Password, string? FullName);
public record LoginRequest(string Username, string Password);
public record TokenResponse(string AccessToken, string RefreshToken);
public record RefreshRequest(string RefreshToken);
public record RevokeRequest(string RefreshToken);