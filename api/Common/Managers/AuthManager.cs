using System.Security.Claims;

namespace api.Common.Managers;

public interface IAuthManager
{
    bool TryGetUserId(out int userId);
}

public class AuthManager(IHttpContextAccessor httpContextAccessor) : IAuthManager
{
    private readonly HttpContext httpContext = httpContextAccessor?.HttpContext
        ?? throw new ArgumentNullException(nameof(httpContextAccessor),
            "HttpContext is null. Ensure IHttpContextAccessor is registered in the DI container.");

    public bool TryGetUserId(out int userId)
    {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out userId))
        {
            userId = 0;
            return false;
        }
        return true;
    }
}
