using System.Collections.Generic;

namespace Equinox.Models.ViewModels
{
    public class EquinoxViewModel
    {
        public List<EquinoxClass> AllClasses { get; set; } = new();
        public List<Club> AllClubs { get; set; } = new();
        public List<ClassCategory> AllCategories { get; set; } = new();

        public string ActiveClubName { get; set; } = "All";
        public string ActiveCategoryName { get; set; } = "All";

    }
}
