namespace Equinox.Models.DTOs
{
    public class EquinoxClassDto
    {
        public int EquinoxClassId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ClassPicture { get; set; }
        public string? ClassDay { get; set; }
        public string? Time { get; set; }
        public string ClubName { get; set; } = string.Empty;
        public string ClassCategoryName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
