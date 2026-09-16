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

        // UNVERIFIED = 0
        // PENDING = 1
        // APPROVED = 2
        // REJECTED = 3


        private readonly IMapper _mapper;
        private readonly SeamsDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        private const int OtpLengthDigits = 6;
        private static readonly TimeSpan OtpValidity = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan OtpResendCooldown = TimeSpan.FromSeconds(30);

        public StudentApplicationService(
             IMapper mapper,
             SeamsDbContext dbContext,
             IPasswordHasher<User> passwordHasher,
             IHttpContextAccessor httpContextAccessor,
             IEmailService emailService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        public async Task<StudentApplicationResponse> CreateStudentApplication(StudentApplicationRequest request)
        {
            var normalizedEmail = request.Email!.Trim().ToLowerInvariant();

            var duplicateEmailApplication = await _dbContext.StudentApplications
                        .AnyAsync(a => a.Email != null
                                    && a.Email.ToLower() == normalizedEmail
                                    && a.Status != 0
                                    && a.Status != 3);

            if (duplicateEmailApplication)
            {
                throw new InvalidOperationException(
                    "An application with this email already exists.");
            }

            var duplicateSchoolId = await _dbContext.StudentApplications
                    .AnyAsync(a => a.SchoolStudentId == request.SchoolStudentId
                                && a.Status != 0
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

            var staleUnverified = await _dbContext.StudentApplications
                .FirstOrDefaultAsync(a =>
                    a.Status == 0 &&
                    ((a.Email != null && a.Email.ToLower() == normalizedEmail)
                     || a.SchoolStudentId == request.SchoolStudentId));

            StudentApplication studentApplication;

            if (staleUnverified != null)
            {
                _mapper.Map(request, staleUnverified);
                staleUnverified.Email = request.Email.Trim();
                staleUnverified.Status = 0; // still UNVERIFIED
                staleUnverified.SubmittedAt = DateTime.UtcNow;
                studentApplication = staleUnverified;
            }
            else
            {
                studentApplication = _mapper.Map<StudentApplication>(request);
                studentApplication.Email = request.Email.Trim();
                studentApplication.Status = 0; // UNVERIFIED — awaiting OTP
                studentApplication.SubmittedAt = DateTime.UtcNow;
                _dbContext.StudentApplications.Add(studentApplication);
            }

            var otp = EmailUtility.GenerateOtp();
            studentApplication.OtpCodeHash = EmailUtility.HashOtp(otp);
            studentApplication.OtpExpiresAt = DateTime.UtcNow.Add(OtpValidity);
            studentApplication.OtpLastSentAt = DateTime.UtcNow;
            studentApplication.EmailVerified = false;

            await _dbContext.SaveChangesAsync();
            await _emailService.SendOtpEmailAsync(studentApplication.Email, otp);
            return _mapper.Map<StudentApplicationResponse>(studentApplication);
        }
        public async Task<int> ApproveStudentApplication(int studentApplicationId)
        {
            var userId = ClaimsUtility.GetUserIdFromClaims(_httpContextAccessor.HttpContext!);

            var existingStudentApplication = await _dbContext.StudentApplications.FindAsync(studentApplicationId);
            if (existingStudentApplication == null)
            {
                return 0;
            }

            // Guard: only pending can be approved
            if (existingStudentApplication.Status != 1)
                throw new InvalidOperationException("Only pending applications can be approved.");

            if (!existingStudentApplication.EmailVerified)
                throw new InvalidOperationException("Applicant has not verified their email.");

            // Guard: email already registered as a user
            var emailInUse = await _dbContext.Users
                .AnyAsync(u => u.Email != null
                            && u.Email.ToLower() == existingStudentApplication.Email!.ToLower());

            if (emailInUse)
                throw new InvalidOperationException("A user with this email already exists.");

            var tempPassword = EmailUtility.GenerateTempPassword();

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            int newStudentId;
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
                    tempPassword);

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

                newStudentId = student.StudentId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            // Send outside the transaction — a slow/failed email should never
            // hold the DB transaction open or roll back a successful approval.
            await _emailService.SendApplicationApprovedEmailAsync(
                existingStudentApplication.Email,
                existingStudentApplication.FirstName,
                tempPassword);

            return newStudentId;

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

        public async Task VerifyEmailAsync(string email, string code)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();

            var application = await _dbContext.StudentApplications
                .FirstOrDefaultAsync(a => a.Email != null
                                        && a.Email.ToLower() == normalizedEmail
                                        && a.Status == 0)
                ?? throw new InvalidOperationException("No pending verification found for this email.");

            if (application.OtpExpiresAt == null || application.OtpExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("Verification code has expired. Please request a new one.");

            if (string.IsNullOrEmpty(application.OtpCodeHash) || !EmailUtility.VerifyOtp(code, application.OtpCodeHash))
                throw new InvalidOperationException("Invalid verification code.");

            application.EmailVerified = true;
            application.Status = 1; // now PENDING — visible to admin
            application.OtpCodeHash = null;
            application.OtpExpiresAt = null;
            application.SubmittedAt = DateTime.UtcNow; // submission is now finalized for admin review

            await _dbContext.SaveChangesAsync();
        }

        public async Task ResendVerificationAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();

            var application = await _dbContext.StudentApplications
                .FirstOrDefaultAsync(a => a.Email != null
                                        && a.Email.ToLower() == normalizedEmail
                                        && a.Status == 0)
                ?? throw new InvalidOperationException("No pending verification found for this email.");

            if (application.OtpLastSentAt != null
                && application.OtpLastSentAt > DateTime.UtcNow.Subtract(OtpResendCooldown))
            {
                throw new InvalidOperationException("Please wait before requesting another code.");
            }

            var otp = EmailUtility.GenerateOtp();
            application.OtpCodeHash = EmailUtility.HashOtp(otp);
            application.OtpExpiresAt = DateTime.UtcNow.Add(OtpValidity);
            application.OtpLastSentAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            await _emailService.SendOtpEmailAsync(application.Email, otp);
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
