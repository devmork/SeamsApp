using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Student;

namespace SeamsApp.Interfaces.Services.Queries
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(string email, string password);
    }
}
