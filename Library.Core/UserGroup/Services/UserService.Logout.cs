using System.Security.Claims;
using Library.Core.HttpModels;

namespace Library.Core.UserGroup.Services;

public partial class UserService
{
    public async Task<AppResponse<bool>> UserLogoutAsync(ClaimsPrincipal user)
    {
        if (!(user.Identity?.IsAuthenticated ?? false)) return new AppResponse<bool>().SetSuccessResponse(true);
        
        var username = user.Claims.First(x => x.Type == "UserName").Value;
        var appUser = _context.Users.First(x => x.UserName == username);
        
        await _userManager.UpdateSecurityStampAsync(appUser);
       
        return new AppResponse<bool>().SetSuccessResponse(true);
    }
}