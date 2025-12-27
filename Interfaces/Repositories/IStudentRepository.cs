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
        Task<int> UpdateStudentAsync(Student student);
        Task<int> DeleteStudentByIdAsync(string schoolStudentId);
        Task<Student> GetStudentByIdAsync(string schoolStudentId);
        Task<Student> GetStudentQRCodeAsync(string schoolStudentId);
    }
}
