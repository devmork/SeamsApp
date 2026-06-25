using SeamsApp.Models;

namespace SeamsApp.Interfaces.Services.Queries
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
