namespace SeamsApp.DTOs.Auth
{
    public class LogInResponseDTO
    {
        public string Token { get; set; } = "";
        public UserDTO User { get; set; } = null!;
    }
}
