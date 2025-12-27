using SeamsApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudentAsync();
        Task<int> AddStudentAsync(Student student);
        Task<int> UpdateStudentByIdAsync(Student student);
        Task<int> DeleteStudentByIdAsync(int studentId);
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<Student> GetStudentQRCodeAsync(string schoolStudentId);
    }
}
