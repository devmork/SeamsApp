using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Student;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    }
}
