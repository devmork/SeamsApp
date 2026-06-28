using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using SeamsApp.Data;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services.Queries;
using SeamsApp.Models;
using SeamsApp.Models.Base;
using SeamsApp.Utilities;
using System.Runtime.Versioning;

namespace SeamsApp.Services.Queries
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

        public Task<int> DeleteStudentByIdAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StudentResponse>> GetAllActiveStudentAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StudentResponse> GetStudentByIdAsync(int studentId)
        {
            throw new NotImplementedException();
        }

        public Task<StudentResponse> GetStudentQRCodeInfoAsync(string schoolStudentId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateStudentByIdAsync(int studentId, StudentRequest studentRequest)
        {
            throw new NotImplementedException();
        }
    }
}
