namespace PollForge.Application.DTOs.Auth;

public record AuthResponse(Guid UserId, string Token, DateTimeOffset ExpiresAt);
