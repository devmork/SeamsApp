using SeamsApp.DTOs.Auth;

namespace SeamsApp.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<RegisterUserDTO> RegisterUserAsync(RegisterUserDTO registerUserDTO);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<LoginResponseDTO> LoginAsync(string email, string password);
    }
}
