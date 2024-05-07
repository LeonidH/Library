using Library.Core.DataServices.UserGroup;
using Library.Core.DataServices.UserGroup.Models;
using Library.Core.DataServices.UserGroup.Services;
using Library.Core.HttpModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    
    [HttpGet]
    [Authorize(Roles = UserGroupConstant.Administrator)]
    public async Task<IActionResult> Get(int page, int pageSize)
    {
        var (users, total) = await _userService.GetAppUsersAsync(page, pageSize);
        return Ok(new
        {
            users,
            total
        });
    }
    
    [HttpPost]
    public async Task<AppResponse<bool>> Register(UserRegisterRequest req)
    {
        return await _userService.UserRegisterAsync(req);
    }

    [HttpPost]
    public async Task<AppResponse<UserLoginResponse>> Login(UserLoginRequest req)
    {
        return await _userService.UserLoginAsync(req);
    }

    [HttpPost]
    public async Task<AppResponse<UserRefreshTokenResponse>> RefreshToken(UserRefreshTokenRequest req)
    {
        return await _userService.UserRefreshTokenAsync(req);
    }
    [HttpPost]
    public async Task<AppResponse<bool>> Logout()
    {
        return await _userService.UserLogoutAsync(User);
    }

    [HttpPost]
    [Authorize]
    public string Profile()
    {
        return User.FindFirst("UserName")?.Value ?? "";
    }
}