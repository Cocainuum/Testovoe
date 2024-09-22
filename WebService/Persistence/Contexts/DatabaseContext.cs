using Microsoft.EntityFrameworkCore;
using WebService.Persistence.Entities;

namespace WebService.Persistence.Contexts;

public class DatabaseContext : DbContext
{
    public DbSet<Medicine> Medicines { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DatabaseContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medicine>()
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}