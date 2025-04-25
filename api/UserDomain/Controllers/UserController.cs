using Microsoft.AspNetCore.Mvc;
using api.UserDomain.DTOs;
using api.UserDomain.Services;

namespace api.UserDomain.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await userService.GetAsync();
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> UpdatePermissions(int id, [FromBody] UpdateUserPermissionsRequest request)
    {
        var response = await userService.UpdatePermissionsAsync(id, request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("{id}/block")]
    public async Task<IActionResult> Block(int id)
    {
        var response = await userService.BlockAsync(id);
        return StatusCode(response.StatusCode, response);
    }
}