using SeamsApp.DTOs.Student;
using SeamsApp.Models.Base;

namespace SeamsApp.Interfaces.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllPendingStudentAsync();
        Task<IEnumerable<StudentDTO>> GetAllApprovedStudentAsync();
        Task<CreateStudentDTO> CreateStudent(CreateStudentDTO createStudentDTO);
        Task<int> UpdateStudentByIdAsync(StudentUpdateDTO studentUpdateDTO, int studentId);
        Task<int> DeleteStudentByIdAsync(int studentId);
        Task<StudentDTO> GetStudentByIdAsync(int studentId);
        Task<StudentDTO> GetStudentQRCodeAsync(string schoolStudentId);
        Task<StudentDTO> ApprovedStudentAsync(int studentId);
        Task<StudentDTO> RejectStudentAsync(int studentId);

    }
}
