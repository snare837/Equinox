using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Equinox.Models
{
    internal class ConfigureMemberships : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> entity)
        {
            entity.HasData(
                new Membership { MembershipId = 1, Name = "Annual", Price = "1000" },
                new Membership { MembershipId = 2, Name = "Monthly", Price = "100" },
                new Membership { MembershipId = 3, Name = "PunchCard", Price = "10" }
            );
        }
    }
}
