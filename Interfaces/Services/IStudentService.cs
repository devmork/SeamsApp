using SeamsApp.DTOs.Student;
using SeamsApp.Models.Base;

namespace SeamsApp.Interfaces.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudentAsync();
        Task<int> RegisterStudentAsync(StudentCreationDTO studentCreationDTO);
        Task<int> UpdateStudentByIdAsync(StudentUpdateDTO studentUpdateDTO, int studentId);
        Task<int> DeleteStudentByIdAsync(int studentId);
        Task<StudentDTO> GetStudentByIdAsync(int studentId);
        Task<StudentDTO> GetStudentQRCodeAsync(string schoolStudentId);

    }
}
