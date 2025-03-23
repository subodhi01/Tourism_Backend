using Microsoft.EntityFrameworkCore;
using TourismGalle.Models;

public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    public DbSet<User> Users { get; set; } // ✅ Add Users Table
    public DbSet<Review> Reviews { get; set; }

}