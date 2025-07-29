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
            // Seed Clubs
            modelBuilder.Entity<Club>().HasData(
                new Club { ClubId = 1, Name = "Chicago Loop", PhoneNumber = "312-987-7223" },
                new Club { ClubId = 2, Name = "West Chicago", PhoneNumber = "982-123-7690" },
                 new Club { ClubId = 3, Name = "Lincoln Park", PhoneNumber = "290-734-1890" }
            );

            // Seed Class Categories
             modelBuilder.Entity<ClassCategory>().HasData(
                new ClassCategory { ClassCategoryId = 1, Name = "Yoga" },
                new ClassCategory { ClassCategoryId = 2, Name = "HIIT" },
                new ClassCategory { ClassCategoryId = 3, Name = "Boxing" }
            );

            // Seed Coaches

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "John Doe",
                    PhoneNumber = "341-897-8129",
                    Email = "johndoe@temp.com",
                    DOB = new DateTime(2000, 3, 11),
                    IsCoach = true
                },
                new User
                {
                    UserId = 2,
                    Name = "Jane Smith",
                    PhoneNumber = "893-129-0910",
                    Email = "janesmith@temp.com",
                    DOB = new DateTime(1986, 6, 25),
                    IsCoach = true
                },
                 new User
                {
                    UserId = 3,
                    Name = "Betty Page",
                    PhoneNumber = "389-090-0010",
                    Email = "bettypage@demo.com",
                    DOB = new DateTime(1075, 8, 17),
                    IsCoach = true
                }
            );

           

            // ✅ Seed ONLY TWO EquinoxClasses as required
            modelBuilder.Entity<EquinoxClass>().HasData(
                new EquinoxClass
                {
                    EquinoxClassId = 1,
                    Name = "Morning Yoga",
                    ClassPicture = "yoga1.jpg",
                    ClassDay = "Monday",
                    Time = "8 AM – 9 AM",
                    ClubId = 1,
                    ClassCategoryId = 1,
                    UserId = 1
                },
                new EquinoxClass
                {
                    EquinoxClassId = 2,
                    Name = "Evening Pilates",
                    ClassPicture = "pilates1.jpg",
                    ClassDay = "Wednesday",
                    Time = "6 PM – 7 PM",
                    ClubId = 2,
                    ClassCategoryId = 2,
                    UserId = 2
                },
                   new EquinoxClass
                {
                    EquinoxClassId = 3,
                    Name = "Boxing Basics",
                    ClassPicture = "boxing1.jpg",
                    ClassDay = "Friday",
                    Time = "5 PM – 6 PM",
                    ClubId = 3,
                    ClassCategoryId = 3,
                    UserId = 3
                }
            );
        }
    }
}
