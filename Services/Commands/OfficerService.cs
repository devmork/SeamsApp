using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SeamsApp.Data;
using SeamsApp.DTOs.AttendanceRecords;
using SeamsApp.DTOs.Officer;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;
using SeamsApp.Utilities;

namespace SeamsApp.Services.Commands
{
    public class OfficerService : IOfficerService
    {
        private readonly SeamsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OfficerService(SeamsDbContext dbContext,
                              IMapper mapper,
                              IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext; 
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> CreateOfficerAsync(int userId)
        {
            var adminId = ClaimsUtility.GetUserIdFromClaims(_httpContextAccessor.HttpContext!);

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return 0;
            }

            user.Role = "Officer";
            _dbContext.Users.Update(user);

            var newOfficer = new Officer
            {
                UserId = user.UserId,
                AddedBy = adminId,
                AddedAt = DateTime.UtcNow,
                Status = 1
            };

            await _dbContext.Officers.AddAsync(newOfficer);
            await _dbContext.SaveChangesAsync();

            return newOfficer.OfficerId;

        }

        public async Task<IEnumerable<OfficerResponse>> GetAllOfficers()
        {
            var officers = await _dbContext.Officers
                .Where(o => o.Status == 1)           
                .Include(o => o.User)
                .Select(o => new OfficerResponse
                {
                    OfficerId = o.OfficerId,
                    UserId = o.UserId,
                    Email = o.User.Email ?? string.Empty,
                    FirstName = o.User.Student != null ? o.User.Student.FirstName ?? string.Empty : string.Empty,
                    MiddleName = o.User.Student != null ? o.User.Student.FirstName ?? string.Empty : string.Empty,
                    LastName = o.User.Student != null ? o.User.Student.FirstName ?? string.Empty : string.Empty,
                    Suffix = o.User.Student != null ? o.User.Student.FirstName ?? string.Empty : string.Empty,
                    SchoolStudentId = o.User.Student != null ? o.User.Student.SchoolStudentId ?? string.Empty : string.Empty,
                    YearLevel = o.User.Student != null ? o.User.Student.YearLevel ?? string.Empty : string.Empty,
                    Course = o.User.Student != null ? o.User.Student.Course ?? string.Empty : string.Empty,
                    PhotoUrl = o.User.Student != null ? o.User.Student.PhotoUrl ?? string.Empty : string.Empty

                })
                .ToListAsync();

            return _mapper.Map<IEnumerable<OfficerResponse>>(officers);
        }

        public async Task<int> RemoveOfficerAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return 0;
            }

            user.Role = "Student";
            _dbContext.Users.Update(user);

            var officer = _dbContext.Officers.FirstOrDefault(o => o.UserId == userId);
            if (officer == null)
            {
                return 0;
            }

            _dbContext.Officers.Remove(officer);
            await _dbContext.SaveChangesAsync();

            return officer.OfficerId;

        }
    }
}
