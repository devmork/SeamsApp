using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Auth
{
    public class CreateAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string PasswordHash { get; set; } = null!;
    }
}
