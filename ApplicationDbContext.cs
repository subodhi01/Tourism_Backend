using Microsoft.EntityFrameworkCore;
using TourismGalle.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } // ✅ Existing Users Table
    public DbSet<Place> Places { get; set; } // ✅ Added Places Table

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Place>()
            .ToTable("Places") // Ensures mapping to the database table
            .HasKey(p => p.Id);
    }
}
