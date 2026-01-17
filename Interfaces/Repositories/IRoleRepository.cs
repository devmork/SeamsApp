using SeamsApp.Models;

namespace SeamsApp.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
    }
}
