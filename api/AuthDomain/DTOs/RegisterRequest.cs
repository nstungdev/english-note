using System;
using System.ComponentModel.DataAnnotations;

namespace api.AuthDomain.DTOs;

public record RegisterRequest
{
    [Required]
    public required string Username { get; init; }
    [Required]
    public required string Email { get; init; }
    [Required]
    public required string Password { get; init; }
    public string? FullName { get; init; } = null;
}
