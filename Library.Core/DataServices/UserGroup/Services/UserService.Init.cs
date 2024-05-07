using System.Security.Claims;
using Library.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Library.Core.DataServices.UserGroup.Services;

public partial class UserService
{
    public async Task InitializeAsync()
    {
        var administratorRole = await CreateRoleAsync(UserGroupConstant.Administrator, new[]
        {
            ("RoleClaim", "HasRoleView"),
            ("RoleClaim", "HasRoleAdd"),
            ("RoleClaim", "HasRoleEdit"),
            ("RoleClaim", "HasRoleDelete")
        });

        var librarianRole = await CreateRoleAsync(UserGroupConstant.Librarian, new[]
        {
            ("RoleClaim", "HasRoleView"),
            ("RoleClaim", "HasRoleAdd"),
            ("RoleClaim", "HasRoleEdit"),
            ("RoleClaim", "HasRoleDelete")
        });

        var readerRole = await CreateRoleAsync(UserGroupConstant.Reader, new[]
        {
            ("RoleClaim", "HasRoleView")
        });
        
        await CreateUserAsync(administratorRole);
        await CreateUserAsync(librarianRole);
        await CreateUserAsync(readerRole);
    }

    private async Task CreateUserAsync(IdentityRole role)
    {
        var user = new ApplicationUser
        {
            UserName = $"{role.Name?.ToLower()}@library.com",
            Email = $"{role.Name?.ToLower()}@library.com",
        };

        if (_userManager.Users.All(x => x.UserName != user.UserName))
        {
            var result = await _userManager.CreateAsync(user, $"Password123!");

            if (result.Succeeded)
            {
                if (role.Name != null)
                    await _userManager.AddToRoleAsync(user, role.Name);

                _logger.LogInformation($"User '{user.UserName}' created successfully.");
            }
            else
            {
                _logger.LogError($"An error occurred while creating user '{user.UserName}'.");
            }
        }
    }

    private async Task<IdentityRole> CreateRoleAsync(string roleName, (string type, string value)[] claims)
    {
        var role = new IdentityRole(roleName);

        if (_roleManager.Roles.All(x => x.Name != role.Name))
        {
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                foreach (var claim in claims)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(claim.type, claim.value));
                }

                _logger.LogInformation($"Role '{role.Name}' created successfully.");
            }
            else
            {
                _logger.LogError($"An error occurred while creating role '{role.Name}'.");
            }
        }

        return role;
    }
}