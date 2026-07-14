using SeamsApp.DTOs.Auth;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string email, string password);
    }
}
