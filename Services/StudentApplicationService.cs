using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SeamsApp.Data;
using SeamsApp.DTOs.StudentApplication;
using SeamsApp.Interfaces.Services;
using SeamsApp.Models;

namespace SeamsApp.Services
{
    public class StudentApplicationService : IStudentApplicationService
    {
        private readonly IMapper _mapper;
        private readonly SeamsDbContext _dbContext;
        public StudentApplicationService(IMapper mapper,SeamsDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<CreateStudentApplicationRequest> CreateStudentApplication(CreateStudentApplicationRequest createStudentApplicationRequest)
        {
            var studentApplication = _mapper.Map<StudentApplication>(createStudentApplicationRequest);
            studentApplication.Status = 1; // PENDING

            _dbContext.StudentApplications.Add(studentApplication);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CreateStudentApplicationRequest>(studentApplication);
        }
        public async Task<int> ApproveStundetApplication(int studentApplicationId)
        {
            var existingStudentApplication = await _dbContext.StudentApplications.FindAsync(studentApplicationId);
            if (existingStudentApplication == null)
            {
                return 0;
            }

            existingStudentApplication.Status = 2; // APPROVED
            _dbContext.StudentApplications.Update(existingStudentApplication);
            return await _dbContext.SaveChangesAsync();

        }
        public async Task<int> RejectStundetApplication(int studentApplicationId)
        {
            var existingStudentApplication = await _dbContext.StudentApplications.FindAsync(studentApplicationId);
            if (existingStudentApplication == null)
            {
                return 0;
            }

            existingStudentApplication.Status = 3; // REJECTED
            _dbContext.StudentApplications.Update(existingStudentApplication);
            return await _dbContext.SaveChangesAsync();
        }
        public Task<IEnumerable<StudentApplicationResponse>> GetAllApproveStudentApplicationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StudentApplicationResponse>> GetAllPendingStudentApplicationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StudentApplicationResponse>> GetAllStudentApplicationsAsync()
        {
            throw new NotImplementedException();
        }

        
    }
}
