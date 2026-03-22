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
        Task<IEnumerable<Student>> GetAllPendingStudentAsync();
        Task<IEnumerable<Student>> GetAllApprovedStudentAsync();
        Task<Student> CreateStudentAsync(Student student);
        Task<int> UpdateStudentByIdAsync(Student student);
        Task<int> DeleteStudentByIdAsync(int studentId);
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<Student> GetStudentQRCodeAsync(string schoolStudentId);
        Task UpdateStudentStatusToApprovedAsync(int studentId, int status, byte[]? qrCode);
        Task<int> UpdateStudentStatusToRejectAsync(int studentId, int status);
    }
}
