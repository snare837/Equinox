using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Equinox.Models
{
    internal class ConfigureEquinoxClasses : IEntityTypeConfiguration<EquinoxClass>
    {
        public void Configure(EntityTypeBuilder<EquinoxClass> entity)
        {
            // prevent cascade delete when Club/Category/Coach has classes
            entity.HasOne(c => c.Club)
                  .WithMany()
                  .HasForeignKey(c => c.ClubId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.ClassCategory)
                  .WithMany()
                  .HasForeignKey(c => c.ClassCategoryId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.User)
                  .WithMany()
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            // seed classes
            entity.HasData(
                new EquinoxClass { EquinoxClassId = 1, Name = "Morning Yoga",  ClassPicture = "yoga1.jpg",   ClassDay = "Monday",    Time = "8 AM – 9 AM", ClubId = 1, ClassCategoryId = 1, UserId = 1 },
                new EquinoxClass { EquinoxClassId = 2, Name = "Evening Pilates",ClassPicture = "pilates1.jpg",ClassDay = "Wednesday", Time = "6 PM – 7 PM", ClubId = 2, ClassCategoryId = 2, UserId = 2 },
                new EquinoxClass { EquinoxClassId = 3, Name = "Boxing Basics",  ClassPicture = "boxing1.jpg", ClassDay = "Friday",    Time = "5 PM – 6 PM", ClubId = 3, ClassCategoryId = 3, UserId = 3 }
            );
        }
    }
}
