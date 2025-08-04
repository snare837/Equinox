using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Please enter a Club name.")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric and cannot contain special characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a phone number.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone number must be in the format XXX-XXX-XXXX.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
