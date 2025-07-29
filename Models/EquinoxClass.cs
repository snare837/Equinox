using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equinox.Models
{
    public class EquinoxClass
    {
        public int EquinoxClassId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? ClassPicture { get; set; }
        public string? ClassDay { get; set; }
        public string? Time { get; set; }

        public int ClassCategoryId { get; set; }
        public ClassCategory ClassCategory { get; set; } = null!;

        public int ClubId { get; set; }
        public Club Club { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
