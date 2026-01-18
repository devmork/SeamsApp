namespace SeamsApp.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = "";
        public UserDTO User { get; set; } = null!;
    }
}
