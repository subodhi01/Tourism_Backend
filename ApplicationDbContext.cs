using Microsoft.EntityFrameworkCore;
using TourismGalle.Models;

namespace TourismGalle.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<TourPackage> TourPackages { get; set; }
    }
}



