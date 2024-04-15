using System.Security.Claims;
using Library.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Data;

public class DbContextInitialiser(
    ILogger<DbContextInitialiser> logger,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    private readonly ILogger<DbContextInitialiser> _logger = logger;
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    
    public async Task InitialiseAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        // try
        // {
        //     if (_context.Database.IsSqlServer())
        //     {
        //         await _context.Database.MigrateAsync();
        //     }
        // }
        // catch (Exception ex)
        // {
        //     _logger.LogError(ex, "An error occurred while running the database migration.");
        //     throw;
        // }
        
        
        if (_context.Users.Any())
        {
            _logger.LogInformation("Users already exist in the database.");
            return;
        }
        
        await CreateRolesAndUsersAsync();
    }
    
    private async Task CreateRolesAndUsersAsync()
    {
        var administratorRole = await CreateRoleAsync("Administrator", new[]
        {
            ("RoleClaim", "HasRoleView"),
            ("RoleClaim", "HasRoleAdd"),
            ("RoleClaim", "HasRoleEdit"),
            ("RoleClaim", "HasRoleDelete")
        });
        
        var librarianRole = await CreateRoleAsync("Librarian", new[]
        {
            ("RoleClaim", "HasRoleView"),
            ("RoleClaim", "HasRoleAdd"),
            ("RoleClaim", "HasRoleEdit"),
            ("RoleClaim", "HasRoleDelete")
        });
        
        var readerRole = await CreateRoleAsync("Reader", new[]
        {
            ("RoleClaim", "HasRoleView")
        });

        var administrator = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@library.com",
        };
        
        if(_userManager.Users.All(x => x.UserName != administrator.UserName))
        {
            var result = await _userManager.CreateAsync(administrator, "Admin123!");
            
            if(result.Succeeded)
            {
                if (administratorRole.Name != null)
                    await _userManager.AddToRoleAsync(administrator, administratorRole.Name);

                _logger.LogInformation($"User '{administrator.UserName}' created successfully.");
            }
            else
            {
                _logger.LogError($"An error occurred while creating user '{administrator.UserName}'.");
            }
        }
    }

    private async Task<IdentityRole> CreateRoleAsync(string roleName, (string type, string value)[] claims)
    {
        var role = new IdentityRole(roleName);
        
        if(_roleManager.Roles.All(x => x.Name != role.Name))
        {
            var result = await _roleManager.CreateAsync(role);
            
            if(result.Succeeded)
            {
                foreach(var claim in claims)
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