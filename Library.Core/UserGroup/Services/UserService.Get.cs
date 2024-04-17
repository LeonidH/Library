using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.UserGroup.Services;

public partial class UserService
{
    public async Task<(List<ApplicationUser> users, int total)> GetAppUsersAsync(int page, int pageSize)
    {
        _userManager.GetUserAsync()
        var users = await _context.Users.Include(x => x.).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        var total = await _context.Users.CountAsync();
        return (users, total);
    }
}