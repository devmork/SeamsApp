using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using SeamsApp.Data;
using SeamsApp.DTOs.StudentApplication;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;
using SeamsApp.Models.Base;
using SeamsApp.Utilities;

namespace SeamsApp.Services.Commands
{
    public class StudentApplicationService : IStudentApplicationService
    {
        private readonly IMapper _mapper;
        private readonly SeamsDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentApplicationService(
             IMapper mapper,
             SeamsDbContext dbContext,
             IPasswordHasher<User> passwordHasher,
             IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
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

            //var userId = ClaimsUtility.GetUserIdFromClaims(_httpContextAccessor.HttpContext!);

            var user = new User
            {
                Email = existingStudentApplication.Email,
                Role = "Student",
                IsActive = 1,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, existingStudentApplication.LastName!.ToUpper());

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var student = new Student
            {
                UserId = user.UserId,
                FirstName = existingStudentApplication.FirstName,
                MiddleName = existingStudentApplication.MiddleName,
                LastName = existingStudentApplication.LastName,
                Suffix = existingStudentApplication.Suffix,
                SchoolStudentId = existingStudentApplication.SchoolStudentId,
                YearLevel = existingStudentApplication.YearLevel,
                Course = existingStudentApplication.Course,
                PhotoUrl = existingStudentApplication.PhotoUrl,
                QRCode = QRCodeUtility.GenerateQRCode(existingStudentApplication.FirstName!, 
                                                      existingStudentApplication.MiddleName!, 
                                                      existingStudentApplication.LastName, 
                                                      existingStudentApplication.Suffix,
                                                      existingStudentApplication.SchoolStudentId!)
            };

            _dbContext.Students.Add(student);

            existingStudentApplication.Status = 2; // APPROVED
            //existingStudentApplication.ReviewedBy = userId;
            existingStudentApplication.ReviewedAt = DateTime.UtcNow;
            _dbContext.StudentApplications.Update(existingStudentApplication);
            return await _dbContext.SaveChangesAsync();

        }
        public async Task<int> RejectStudentApplication(int studentApplicationId)
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
        public async Task<IEnumerable<StudentApplicationResponse>> GetAllPendingStudentApplicationsAsync()
        {
            var approvedApplications = await _dbContext.StudentApplications.Where(app => app.Status == 1).ToListAsync();
            var response = _mapper.Map<IEnumerable<StudentApplicationResponse>>(approvedApplications);
            return response;
        }
        public async Task<IEnumerable<StudentApplicationResponse>> GetAllApprovedStudentApplicationsAsync()
        {
            var approvedApplications = await _dbContext.StudentApplications.Where(app => app.Status == 2).ToListAsync();
            var response = _mapper.Map<IEnumerable<StudentApplicationResponse>>(approvedApplications);
            return response;
        }

        public async Task<IEnumerable<StudentApplicationResponse>> GetAllRejectedStudentApplicationsAsync()
        {
            var rejectApplications = await _dbContext.StudentApplications.Where(app => app.Status == 3).ToListAsync();
            var response = _mapper.Map<IEnumerable<StudentApplicationResponse>>(rejectApplications);
            return response;
        }

        public async Task<IEnumerable<StudentApplicationResponse>> GetAllStudentApplicationsAsync()
        {
            var studentApplications = await _dbContext.StudentApplications.ToListAsync();
            var response = _mapper.Map<IEnumerable<StudentApplicationResponse>>(studentApplications);
            return response;
        }

        
    }
}
