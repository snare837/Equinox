using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Equinox.Models
{
    internal class ConfigureUsers : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasData(
                new User { UserId = 1, Name = "John Doe",  PhoneNumber = "341-897-8129", Email = "johndoe@temp.com",  DOB = new DateTime(2000, 3, 11), IsCoach = true },
                new User { UserId = 2, Name = "Jane Smith", PhoneNumber = "893-129-0910", Email = "janesmith@temp.com", DOB = new DateTime(1986, 6, 25), IsCoach = true },
                new User { UserId = 3, Name = "Betty Page", PhoneNumber = "389-090-0010", Email = "bettypage@demo.com",  DOB = new DateTime(1075, 8, 17), IsCoach = true }
            );
        }
    }
}
