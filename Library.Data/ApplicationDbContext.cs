using Library.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Entities.Library> Libraries { get; set; }
    
    public DbSet<Entities.Book> Books { get; set; }
    
    public DbSet<Entities.BookInstance> BookInstances { get; set; }
    
    public DbSet<Entities.BookRequest> BookRequests { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}