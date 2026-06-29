namespace SeamsApp.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public UserDTO User { get; set; } = null!;
    }
}
