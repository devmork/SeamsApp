using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Student;

namespace SeamsApp.Interfaces.Services.Queries
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<LoginResponseDTO> LoginAsync(string email, string password);
        Task<CreateAdminDTO> CreateUserAsync(CreateAdminDTO createAdminDTO);
    }
}
