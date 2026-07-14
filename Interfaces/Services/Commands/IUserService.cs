using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Student;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IUserService
    {
        Task<UserRequest> GetUserByEmailAsync(string email);
        Task<UserRequest> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserRequest>> GetAllUsersAsync();
    }
}
