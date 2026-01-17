using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.Auth
{
    public class CreateUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
