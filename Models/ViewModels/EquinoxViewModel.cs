using System.Collections.Generic;

namespace Equinox.Models.ViewModels
{
    public class EquinoxViewModel
    {
        // Main data used by views
        public EquinoxClass EquinoxClass { get; set; } = new EquinoxClass();  // for Details view

        public List<EquinoxClass> AllClasses { get; set; } = new();           // for Index list
        public List<Club> AllClubs { get; set; } = new();                     // dropdowns / filters
        public List<ClassCategory> AllCategories { get; set; } = new();

        // State-tracking for UI filters
        public string ActiveClubName { get; set; } = "All";
        public string ActiveCategoryName { get; set; } = "All";

        // View helper methods
        public string CheckActiveClub(string club) =>
            club.ToLower() == ActiveClubName.ToLower() ? "active" : "";

        public string CheckActiveCategory(string category) =>
            category.ToLower() == ActiveCategoryName.ToLower() ? "active" : "";
    }
}
