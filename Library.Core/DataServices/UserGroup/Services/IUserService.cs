using System.Security.Claims;
using Library.Core.DataServices.UserGroup.Models;
using Library.Core.HttpModels;
using Library.Data.Entities;

namespace Library.Core.DataServices.UserGroup.Services;

public interface IUserService
{
    Task<(List<ApplicationUser> users, int total)> GetAppUsersAsync(int page, int pageSize);
    Task<AppResponse<UserLoginResponse>> UserLoginAsync(UserLoginRequest request);
    Task<AppResponse<bool>> UserLogoutAsync(ClaimsPrincipal user);
    Task<AppResponse<UserRefreshTokenResponse>> UserRefreshTokenAsync(UserRefreshTokenRequest request);
    Task<AppResponse<bool>> UserRegisterAsync(UserRegisterRequest request);
    Task InitializeAsync();
}