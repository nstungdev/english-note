using Microsoft.AspNetCore.Mvc;
using api.AuthDomain.Services;
using api.AuthDomain.DTOs;

namespace api.AuthDomain.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await authService.RegisterUserAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await authService.RefreshTokenAsync(request.Token);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
    {
        var response = await authService.RevokeRefreshTokenAsync(request.Token);
        return StatusCode(response.StatusCode, response);
    }
}