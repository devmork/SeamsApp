using SeamsApp.DTOs.Student;
using SeamsApp.Models.Base;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponse>> GetAllActiveStudentAsync();
        Task<int> UpdateStudentByIdAsync(int studentId, StudentRequest studentRequest);
        Task<int> DeleteStudentByIdAsync(int studentId);
        Task<StudentResponse> GetStudentByIdAsync(int studentId);
        Task<StudentResponse> GetStudentQRCodeInfoAsync(string schoolStudentId);

    }
}
