using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Data;

public class DbContextInitialiser(
    ILogger<DbContextInitialiser> logger,
    ApplicationDbContext context)
{
    private readonly ILogger<DbContextInitialiser> _logger = logger;
    private readonly ApplicationDbContext _context = context;

    public async Task InitialiseAsync()
    {
        //await _context.Database.EnsureCreatedAsync();

        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while running the database migration.");
            throw;
        }
        
        if (_context.Users.Any())
        {
            _logger.LogInformation("Users already exist in the database.");
        }
    }
}