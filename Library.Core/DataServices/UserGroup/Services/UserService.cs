﻿using System.Security.Claims;
using Library.Core.DataServices.UserGroup.Models;
using Library.Core.DataServices.UserGroup.Utils;
using Library.Core.Options;
using Library.Data;
using Library.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Library.Core.DataServices.UserGroup.Services;

public partial class UserService(
    ILogger<UserService> logger,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<IdentityRole> roleManager,
    ApplicationDbContext applicationDbContext,
    TokenConfig tokenConfig) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly TokenConfig _tokenConfig = tokenConfig;
    private readonly ApplicationDbContext _context = applicationDbContext;

    private async Task<UserLoginResponse> GenerateUserToken(ApplicationUser user)
    {
        var claims = (from ur in _context.UserRoles
                where ur.UserId == user.Id
                join r in _context.Roles on ur.RoleId equals r.Id
                join rc in _context.RoleClaims on r.Id equals rc.RoleId
                select rc)
            .Where(rc => !string.IsNullOrEmpty(rc.ClaimValue) && !string.IsNullOrEmpty(rc.ClaimType))
            .Select(rc => new Claim(rc.ClaimType!, rc.ClaimValue!))
            .Distinct()
            .ToList();

        var roleClaims = (from ur in _context.UserRoles
                where ur.UserId == user.Id
                join r in _context.Roles on ur.RoleId equals r.Id
                select r)
            .Where(r => !string.IsNullOrEmpty(r.Name))
            .Select(r => new Claim(ClaimTypes.Role, r.Name!))
            .Distinct()
            .ToList();

        claims.AddRange(roleClaims);

        var token = TokenUtil.GetToken(_tokenConfig, user, claims);
        await _userManager.RemoveAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
        var refreshToken = await _userManager.GenerateUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
        await _userManager.SetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", refreshToken);
        return new UserLoginResponse() { AccessToken = token, RefreshToken = refreshToken };
    }
}