using SeamsApp.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Interfaces.Repositories
{
    public interface IStudentsRepository
    {
        Task<List<Student>> GetAllStudent();
        Task<int> AddStudent(Student student);
        Task<int> UpdateStudent(Student student);
        Task<Student> GetStudentById(string schoolStudentID);
        Task<Student> GetStudentQRCode(string schoolStudentID);
    }
}
