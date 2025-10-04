using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceManagementSystem.Models.Base;

namespace AttendanceManagementSystem.Interfaces.Repositories
{
    public interface IStudentsRepository
    {
        List<Student> GetAllStudent();
        List<Student> GetStudentQRCode(string schoolStudentId);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        int GetTotalStudents();

        Student GetStudentById(string schoolStudentId);
        bool CheckDuplicateSchoolId(string schoolStudentId, int id);

    }
}
