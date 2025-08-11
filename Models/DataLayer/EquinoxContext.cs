using Microsoft.EntityFrameworkCore;

namespace Equinox.Models
{
    public class EquinoxContext : DbContext
    {
        public EquinoxContext(DbContextOptions<EquinoxContext> options) : base(options) { }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClassCategory> ClassCategories { get; set; }
        public DbSet<User> Coaches { get; set; }
        public DbSet<EquinoxClass> EquinoxClasses { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigureClubs());
            modelBuilder.ApplyConfiguration(new ConfigureClassCategories());
            modelBuilder.ApplyConfiguration(new ConfigureUsers());
            modelBuilder.ApplyConfiguration(new ConfigureEquinoxClasses());
        }
    }
}
