using SeamsApp.DTOs.Auth;

namespace SeamsApp.Interfaces.Services
{
    public interface IAuthService
    {
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<RegisterUserDTO> CreateUserAsync(RegisterUserDTO createUserDTO);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<LoginResponseDTO> LoginAsync(string email, string password);
        Task AssignRoleAsync(int userId, int roleId);
    }
}
