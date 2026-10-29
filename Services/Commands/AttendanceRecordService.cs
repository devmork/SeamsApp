using Microsoft.EntityFrameworkCore;
using SeamsApp.Data;
using SeamsApp.DTOs.AttendanceRecords;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;

namespace SeamsApp.Services.Commands
{
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private readonly SeamsDbContext _dbContext;

        public AttendanceRecordService(SeamsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AttendanceRecordResponse> CreateAttendanceRecordAsync(AttendanceRecordRequest attendanceRecordRequest)
        {
            var attendance = await _dbContext.Attendances.FindAsync(attendanceRecordRequest.AttendanceID);
            if (attendance == null)
            {
                return null!;
            }

            var alreadyLogged = await _dbContext.AttendanceRecords
                .AnyAsync(r => r.AttendanceID == attendanceRecordRequest.AttendanceID
                            && r.SchoolStudentID == attendanceRecordRequest.SchoolStudentID);

            if (alreadyLogged)
            {
                return null!;
            }

            var newRecord = new AttendanceRecord
            {
                AttendanceID = attendanceRecordRequest.AttendanceID,
                SchoolStudentID = attendanceRecordRequest.SchoolStudentID,
                Status = attendanceRecordRequest.Status,
                Timestamp = DateTime.UtcNow
            };

            await _dbContext.AttendanceRecords.AddAsync(newRecord);
            await _dbContext.SaveChangesAsync();

            return await GetAttendanceRecordByIdAsync(newRecord.RecordID);
        }

        public async Task<AttendanceRecordResponse> DeleteAttendanceRecordAsync(int recordId)
        {
            var record = await _dbContext.AttendanceRecords.FindAsync(recordId);
            if (record == null)
            {
                return null!;
            }

            var response = await GetAttendanceRecordByIdAsync(recordId);

            _dbContext.AttendanceRecords.Remove(record);
            await _dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<IEnumerable<AttendanceRecordResponse>> GetAllAttendanceRecordsAsync()
        {
            return await BuildResponseQuery().ToListAsync();
        }

        public async Task<AttendanceRecordResponse> GetAttendanceRecordByIdAsync(int recordId)
        {
            var record = await BuildResponseQuery()
                .FirstOrDefaultAsync(r => r.RecordID == recordId);

            return record!;
        }

        public async Task<IEnumerable<AttendanceRecordResponse>> GetAttendanceRecordsByAttendanceIdAsync(int attendanceId)
        {
            return await BuildResponseQuery()
                .Where(r => r.AttendanceID == attendanceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventAttendanceGroupResponse>> GetStudentAttendanceHistoryAsync(int userId)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null || string.IsNullOrEmpty(student.SchoolStudentId))
            {
                return Enumerable.Empty<EventAttendanceGroupResponse>();
            }

            var now = DateTime.UtcNow;
            var studentYearLevel = student.YearLevel?.Trim();

            // Fetch active sessions + events, then filter scope in memory
            // (string.Split on a column is not reliably translated by EF Core).
            var sessions = await _dbContext.Attendances
                .Include(a => a.Event)
                .Where(a => a.Event.Status == 1)
                .ToListAsync();

            var visibleSessions = sessions
                .Where(a => EventAppliesToYearLevel(a.Event, studentYearLevel))
                .ToList();

            var myRecords = await _dbContext.AttendanceRecords
                .Where(r => r.SchoolStudentID == student.SchoolStudentId)
                .ToListAsync();

            var recordsByAttendanceId = myRecords
                .GroupBy(r => r.AttendanceID)
                .ToDictionary(g => g.Key, g => g.First());

            return visibleSessions
                .GroupBy(a => a.EventId)
                .Select(g => new EventAttendanceGroupResponse
                {
                    EventId = g.Key,
                    EventTitle = g.First().Event?.Title ?? string.Empty,
                    Date = g.First().Date,
                    Sessions = g
                        .OrderBy(a => a.StartTime)
                        .Select(a =>
                        {
                            recordsByAttendanceId.TryGetValue(a.AttendanceId, out var record);
                            return new AttendanceSessionMarkResponse
                            {
                                RecordId = record?.RecordID ?? -a.AttendanceId,
                                AttendanceId = a.AttendanceId,
                                Session = a.Session,
                                StartTime = a.StartTime,
                                EndTime = a.EndTime,
                                Status = record != null ? 1 : (now < a.EndTime ? 2 : 0),
                            };
                        })
                        .ToList(),
                })
                .OrderByDescending(g => g.Date)
                .ToList();
        }

        private static bool EventAppliesToYearLevel(Event ev, string? studentYearLevel)
        {
            // No scope ? visible to everyone.
            if (string.IsNullOrWhiteSpace(ev.TargetYearLevels))
                return true;

            // Scoped event, but the student has no year level ? hide.
            if (string.IsNullOrWhiteSpace(studentYearLevel))
                return false;

            return ev.TargetYearLevels
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Any(s => string.Equals(s, studentYearLevel.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task<AttendanceRecordResponse> UpdateAttendanceRecordAsync(int recordId, AttendanceRecordRequest attendanceRecordRequest)
        {
            var record = await _dbContext.AttendanceRecords.FindAsync(recordId);
            if (record == null)
            {
                return null!;
            }

            record.SchoolStudentID = attendanceRecordRequest.SchoolStudentID;
            record.Status = attendanceRecordRequest.Status;

            _dbContext.AttendanceRecords.Update(record);
            await _dbContext.SaveChangesAsync();

            return await GetAttendanceRecordByIdAsync(recordId);
        }

        // Joins on SchoolStudentID since AttendanceRecord has no direct FK to Student
        private IQueryable<AttendanceRecordResponse> BuildResponseQuery()
        {
            return from r in _dbContext.AttendanceRecords
                   join s in _dbContext.Students on r.SchoolStudentID equals s.SchoolStudentId into studentJoin
                   from student in studentJoin.DefaultIfEmpty()
                   orderby r.Timestamp descending
                   select new AttendanceRecordResponse
                   {
                       RecordID = r.RecordID,
                       AttendanceID = r.AttendanceID,
                       SchoolStudentID = r.SchoolStudentID ?? string.Empty,
                       FullName = student != null
                           ? $"{student.FirstName} {student.LastName}".Trim()
                           : string.Empty,
                       YearLevel = student != null ? student.YearLevel : null,
                       Course = student != null ? student.Course : null,
                       Status = r.Status,
                       Timestamp = r.Timestamp
                   };
        }
    }
}