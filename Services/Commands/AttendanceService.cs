using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SeamsApp.Data;
using SeamsApp.DTOs.Attendance;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models.Base;
using SeamsApp.Utilities;

namespace SeamsApp.Services.Commands
{
    public class AttendanceService : IAttendanceService
    {
        private readonly SeamsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttendanceService(
                            SeamsDbContext dbContext,
                            IMapper mapper,
                            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<AttendanceRequest> CreateAttendanceAsync(int eventId, AttendanceRequest attendanceRequest)
        {

            var userId = ClaimsUtility.GetUserIdFromClaims(_httpContextAccessor.HttpContext!);

            var newAttendance = _mapper.Map<Attendance>(attendanceRequest);

            newAttendance.EventId = eventId;
            newAttendance.Status = 1;
            newAttendance.CreatedBy = userId;

            await _dbContext.Attendances.AddAsync(newAttendance);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<AttendanceRequest>(newAttendance);
        }
        public async Task<AttendanceResponse> DeleteAttendanceAsync(int attendanceId)
        {
            var userId = ClaimsUtility.GetUserIdFromClaims(_httpContextAccessor.HttpContext!);

            var attendance = await _dbContext.Attendances.FindAsync(attendanceId);

            if (attendance == null)
            {
                return null!;
            }

            attendance.Status = 0;
            _dbContext.Attendances.Update(attendance);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<AttendanceResponse>(attendance);
        }

        public async Task<IEnumerable<AttendanceResponse>> GetAllAttendanceAsync()
        {
            var attendances = await _dbContext.Attendances
                                .Where(a => a.Status == 1)
                                .ToListAsync();

            var response = _mapper.Map<IEnumerable<AttendanceResponse>>(attendances);
            return response;
        }

        public async Task<AttendanceResponse> GetAttendanceByIdAsync(int attendanceId)
        {
            var attendance = await _dbContext.Attendances
                                        .FirstOrDefaultAsync(a => a.AttendanceId == attendanceId
                                            && a.Status == 1);

            if (attendance == null)
            {
                return null!;
            }

            var response = _mapper.Map<AttendanceResponse>(attendance);
            return response;
        }

        public async Task<AttendanceResponse> UpdateAttendanceAsync(int attendanceId, AttendanceRequest attendanceRequest)
        {
            var attendance = await _dbContext.Attendances.FindAsync(attendanceId);

            if (attendance!.Status == 0)
            {
                Console.WriteLine("Attendance is deleted and cannot be updated.");
            }

            attendance.Title = attendanceRequest.Title;
            attendance.Date = attendanceRequest.Date;
            attendance.Session = attendanceRequest.Session;
            attendance.LogType = attendanceRequest.LogType;
            attendance.StartTime = attendanceRequest.StartTime;
            attendance.EndTime = attendanceRequest.EndTime;

            _dbContext.Attendances.Update(attendance);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<AttendanceResponse>(attendance);
        }
    }
}
