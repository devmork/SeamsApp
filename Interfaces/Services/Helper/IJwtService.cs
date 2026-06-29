using SeamsApp.Models;

namespace SeamsApp.Interfaces.Services.Helper
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
