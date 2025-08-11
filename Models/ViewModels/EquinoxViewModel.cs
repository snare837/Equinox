using Equinox.Models.DTOs;
using System.Collections.Generic;

namespace Equinox.Models.ViewModels
{
    public class EquinoxViewModel
    {
        public EquinoxClassDto EquinoxClass { get; set; } = new EquinoxClassDto();

        public List<EquinoxClassDto> AllClasses { get; set; } = new List<EquinoxClassDto>();
        public List<Club> AllClubs { get; set; } = new();
        public List<ClassCategory> AllCategories { get; set; } = new();

        public string ActiveClubName { get; set; } = "All";
        public string ActiveCategoryName { get; set; } = "All";

        public string CheckActiveClub(string club) =>
            club.ToLower() == ActiveClubName.ToLower() ? "active" : "";

        public string CheckActiveCategory(string category) =>
            category.ToLower() == ActiveCategoryName.ToLower() ? "active" : "";
    }
}
