using SeamsApp.DTOs.AttendanceRecords;
using SeamsApp.DTOs.Officer;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IOfficerService
    {
        Task<int> CreateOfficerAsync(int userId);
        Task<int> RemoveOfficerAsync(int userId);
        Task<IEnumerable<OfficerResponse>> GetAllOfficers();
    }
}
