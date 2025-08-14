using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Equinox.Models
{
    internal class ConfigureClassCategories : IEntityTypeConfiguration<ClassCategory>
    {
        public void Configure(EntityTypeBuilder<ClassCategory> entity)
        {
            entity.HasData(
                new ClassCategory { ClassCategoryId = 1, Name = "Yoga" },
                new ClassCategory { ClassCategoryId = 2, Name = "HIIT" },
                new ClassCategory { ClassCategoryId = 3, Name = "Boxing" }
            );
        }
    }
}
