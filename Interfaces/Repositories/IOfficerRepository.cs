using SeamsApp.DTOs.Officer;
using SeamsApp.Models.Base;

namespace SeamsApp.Interfaces.Repositories
{
    public interface IOfficerRepository
    {
        Task<IEnumerable<OfficerDTO>> GetAllOfficersAsync();
        Task<int> UpdateStudentStatusToOfficer(int studentId, int status);
        Task<int> RemoveOfficerAsync(int studentId, int status);
    }
}
