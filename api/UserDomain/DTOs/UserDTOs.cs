namespace api.UserDomain.DTOs;

public class UpdateUserPermissionsRequest
{
    public IEnumerable<int> PermissionIds { get; set; } = [];
    public string Operation { get; set; } = "add"; // add or remove
}

public class UserResponse
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public IEnumerable<string> Groups { get; set; } = [];
    public bool IsBlocked { get; set; }
}