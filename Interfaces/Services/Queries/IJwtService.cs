using SeamsApp.Models;

namespace SeamsApp.Interfaces.Services.Queries
{
    public interface IJwtService
    {
        string GenerateTokenAsync(User user);
    }
}
