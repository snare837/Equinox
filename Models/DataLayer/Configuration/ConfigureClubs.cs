using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Equinox.Models
{
    internal class ConfigureClubs : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> entity)
        {
            entity.HasData(
                new Club { ClubId = 1, Name = "Chicago Loop", PhoneNumber = "312-987-7223" },
                new Club { ClubId = 2, Name = "West Chicago", PhoneNumber = "982-123-7690" },
                new Club { ClubId = 3, Name = "Lincoln Park", PhoneNumber = "290-734-1890" }
            );
        }
    }
}
