using SeamsApp.Models;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
