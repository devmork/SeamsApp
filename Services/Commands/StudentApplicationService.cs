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

        // APPROVED = 2
        // REJECTED = 3
        // PENDING = 1

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
        public async Task<StudentApplicationResponse> CreateStudentApplication(CreateStudentApplicationRequest request)
        {
            var normalizedEmail = request.Email!.Trim().ToLowerInvariant();

            var duplicateEmailApplication = await _dbContext.StudentApplications
                        .AnyAsync(a => a.Email != null
                                    && a.Email.ToLower() == normalizedEmail
                                    && a.Status != 3);

            if (duplicateEmailApplication)
            {
                throw new InvalidOperationException(
                    "An application with this email already exists.");
            }

            var duplicateSchoolId = await _dbContext.StudentApplications
                    .AnyAsync(a => a.SchoolStudentId == request.SchoolStudentId
                                && a.Status != 3);

            if (duplicateSchoolId)
            {
                throw new InvalidOperationException(
                    "An application with this School Student ID already exists.");
            }

            var existingUser = await _dbContext.Users
                    .AnyAsync(u => u.Email != null
                                && u.Email.ToLower() == normalizedEmail);

            if (existingUser)
            {
                throw new InvalidOperationException(
                    "An account with this email already exists.");
            }

            var studentApplication = _mapper.Map<StudentApplication>(request);
            studentApplication.Email = request.Email.Trim();
            studentApplication.Status = 1; // PENDING
            studentApplication.SubmittedAt = DateTime.UtcNow;

            _dbContext.StudentApplications.Add(studentApplication);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<StudentApplicationResponse>(studentApplication);
        }
        public async Task<int> ApproveStudentApplication(int studentApplicationId)
        {
            var existingStudentApplication = await _dbContext.StudentApplications.FindAsync(studentApplicationId);
            if (existingStudentApplication == null)
            {
                return 0;
            }

            // Guard: only pending can be approved
            if (existingStudentApplication.Status != 1)
                throw new InvalidOperationException("Only pending applications can be approved.");

            // Guard: last name required to derive a default password
            if (string.IsNullOrWhiteSpace(existingStudentApplication.LastName))
                throw new InvalidOperationException("Application is missing a last name.");

            var userId = ClaimsUtility.GetUserIdFromClaims(_httpContextAccessor.HttpContext!);

            // Guard: email already registered as a user
            var emailInUse = await _dbContext.Users
                .AnyAsync(u => u.Email != null
                            && u.Email.ToLower() == existingStudentApplication.Email!.ToLower());

            if (emailInUse)
                throw new InvalidOperationException("A user with this email already exists.");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = new User
                {
                    Email = existingStudentApplication.Email,
                    Role = "Student",
                    IsActive = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                user.PasswordHash = _passwordHasher.HashPassword(
                    user,
                    existingStudentApplication.LastName.ToUpper());

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
                    QRCode = QRCodeUtility.GenerateQRCode(
                        existingStudentApplication.FirstName,
                        existingStudentApplication.MiddleName ?? string.Empty,
                        existingStudentApplication.LastName,
                        existingStudentApplication.Suffix,
                        existingStudentApplication.SchoolStudentId),
                    Status = 1, // ACTIVE
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _dbContext.Students.Add(student);

                existingStudentApplication.Status = 2;
                existingStudentApplication.ReviewedBy = userId;
                existingStudentApplication.ReviewedAt = DateTime.UtcNow;

                _dbContext.StudentApplications.Update(existingStudentApplication);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return student.StudentId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }
        public async Task<int> RejectStudentApplication(int studentApplicationId)
        {
            var existingStudentApplication = await _dbContext.StudentApplications.FindAsync(studentApplicationId);
            if (existingStudentApplication == null)
            {
                return 0;
            }

            if (existingStudentApplication.Status != 1)
                throw new InvalidOperationException("Only pending applications can be rejected.");

            var userId = ClaimsUtility.GetUserIdFromClaims(_httpContextAccessor.HttpContext!);

            existingStudentApplication.Status = 3;
            existingStudentApplication.ReviewedBy = userId;
            existingStudentApplication.ReviewedAt = DateTime.UtcNow;

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
