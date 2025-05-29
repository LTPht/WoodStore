using System.ComponentModel.DataAnnotations;

namespace WoodStore.Models
{
    public class Customer
    {
        [Key]
        public string Uid { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string PasswordHash { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string? ProfilePictureUrl { get; set; }
        public bool IsAdmin { get; set; }
    }
}
