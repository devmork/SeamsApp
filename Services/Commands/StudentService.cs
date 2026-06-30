using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using SeamsApp.Data;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;
using SeamsApp.Models.Base;
using SeamsApp.Utilities;
using System.Runtime.Versioning;

namespace SeamsApp.Services.Commands
{
    public class StudentService : IStudentService
    {
        private readonly SeamsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentService(SeamsDbContext dbContext,
                              IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> DeleteStudentByIdAsync(int studentId)
        {
            var student = await _dbContext.Students.FindAsync(studentId);
            if (student == null) return 0;

            _dbContext.Students.Update(student);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<StudentResponse>> GetAllActiveStudentAsync()
        {
            var students = await _dbContext.Students.ToListAsync();
            if (students == null)
            {
                return null!;
            }
            return _mapper.Map<IEnumerable<StudentResponse>>(students);
        }

        public async Task<StudentResponse> GetStudentByIdAsync(int studentId)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
            {
                return null!;
            }
            
            var response = _mapper.Map<StudentResponse>(student);
            return response;
        }

        public async Task<StudentResponse> GetStudentQRCodeInfoAsync(string schoolStudentId)
        {
            // Find student by schoolStudentId
            var student = await _dbContext.Students
                .FirstOrDefaultAsync(s => s.SchoolStudentId == schoolStudentId);

            if (student == null)
            {
                return null!;
            }

            // Generate QR code
            var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(student.SchoolStudentId!, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrData).GetGraphic(20);

            // Map to response DTO
            var response = _mapper.Map<StudentResponse>(student);
            response. = qrCode; // ensure StudentResponse has this property

            return response;
        }

        public Task<int> UpdateStudentByIdAsync(int studentId, StudentRequest studentRequest)
        {
            throw new NotImplementedException();
        }
    }
}
