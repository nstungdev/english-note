using System;

namespace api.AuthDomain.DTOs;

public record AuthResponse
{
    public required int UserId { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}
