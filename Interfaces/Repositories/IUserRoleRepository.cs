using SeamsApp.Models;

namespace SeamsApp.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(int userId);
        Task<int> AssignRoleAsync(UserRole userRole);
        Task<List<string>> GetUserRolesAsync(int userId);
        Task RemoveRoleAsync(int userId, int roleId);
    }
}
