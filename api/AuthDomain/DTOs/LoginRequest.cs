using System;
using System.ComponentModel.DataAnnotations;

namespace api.AuthDomain.DTOs;

public record LoginRequest
{
    [Required]
    public required string UsernameOrEmail { get; set; }
    [Required]
    public required string Password { get; set; }
}
