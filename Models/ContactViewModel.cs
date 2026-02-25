using System.ComponentModel.DataAnnotations;

namespace Beyawned.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Industry is required")]
        public string Industry { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;
    }
}
