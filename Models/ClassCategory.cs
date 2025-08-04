using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;  // for NotMapped attribute
using Microsoft.AspNetCore.Mvc;                      // for Remote attribute
namespace Equinox.Models

{
    public class ClassCategory
    {
        public int ClassCategoryId { get; set; }

        [Required(ErrorMessage = "Please enter a ClassCategory name.")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric and cannot contain special characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter an image name.")]
         public string Image { get; set; } = string.Empty;


    }

}

