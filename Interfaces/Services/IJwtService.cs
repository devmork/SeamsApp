using SeamsApp.Models;

namespace SeamsApp.Interfaces.Services
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
