using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a phone number.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone must be in format XXX-XXX-XXXX.")]
        [Remote("CheckPhoneNumber", "Validation",AdditionalFields = nameof(UserId), ErrorMessage = "Phone number already exists.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an email.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Remote("CheckEmail", "Validation", AdditionalFields = nameof(UserId),ErrorMessage = "Email already in use.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your date of birth.")]
        [AgeRange(8, 80, ErrorMessage = "Age must be between 8 and 80.")]
        public DateTime? DOB { get; set; }

        public bool IsCoach { get; set; } = false;
    }
}
