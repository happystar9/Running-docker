using System.Security.Principal;

namespace TetrisWeb.Services;

public class UserIdentityService(IHttpContextAccessor httpContextAccessor)
{
    private async Task<IIdentity> GetUserIdentity()
    {
        var user = httpContextAccessor.HttpContext.User.Identity;
        return user;
    }

}
