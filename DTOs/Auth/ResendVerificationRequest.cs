using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Auth
{
    public class ResendVerificationRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
    }
}
