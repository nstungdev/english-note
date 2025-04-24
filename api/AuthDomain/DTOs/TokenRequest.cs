using System;
using System.ComponentModel.DataAnnotations;

namespace api.AuthDomain.DTOs;

public record RefreshTokenRequest
{
    [Required]
    public required string Token { get; init; }
}

public record RevokeTokenRequest
{
    [Required]
    public required string Token { get; init; }
}
