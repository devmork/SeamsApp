using AutoMapper;
using SeamsApp.DTOs.Attendance;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services;
using SeamsApp.Models;
using SeamsApp.Models.Base;

namespace SeamsApp.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;

        public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAttendance(CreateAttendanceDTO createAttendanceDTO)
        {
            if (createAttendanceDTO == null)
                throw new ArgumentNullException(nameof(createAttendanceDTO));

            if (createAttendanceDTO.StartTime >= createAttendanceDTO.EndTime)
                throw new ArgumentException("Start time must be earlier than end time.");

            var attendance = _mapper.Map<Attendance>(createAttendanceDTO);
            attendance.CreatedAt = DateTime.UtcNow;
            attendance.Status = 1; // Added: Set active status

            int newId = await _attendanceRepository.AddAttendance(attendance);
            return newId;
        }

        public async Task<bool> DeleteAttendance(int attendanceId)
        {
            int rowsAffected = await _attendanceRepository.DeleteAttendance(attendanceId);
            return rowsAffected > 0;
        }

        public async Task<List<AttendanceDTO>> GetAllAttendanceAsync()
        {
            var attendanceList = await _attendanceRepository.GetAllAttendance();
            return _mapper.Map<List<AttendanceDTO>>(attendanceList);
        }

        public async Task<AttendanceDTO?> GetAttendanceByIdAsync(int id)
        {
            var attendance = await _attendanceRepository.GetAttendanceById(id);
            if (attendance == null)
            {
                return null;
            }
            return _mapper.Map<AttendanceDTO>(attendance);
        }

        public async Task<bool> RecordStudentAttendance(int attendanceId, int studentId)
        {
            bool alreadyRecorded = await _attendanceRepository.CheckDuplicateRecord(attendanceId, studentId);
            if (alreadyRecorded)
                return false;

            var record = new AttendanceRecord
            {
                AttendanceID = attendanceId,
                StudentID = studentId,
                Timestamp = DateTime.UtcNow, // Fixed: Use Timestamp (matches DB)
                Status = 1 // Added: Set active status
            };

            int newRecordId = await _attendanceRepository.RecordStudentAttendance(record);
            return newRecordId > 0;
        }

        public async Task<bool> UpdateAttendance(int id, UpdateAttendanceDTO updateAttendanceDTO)
        {
            if (updateAttendanceDTO == null)
                throw new ArgumentNullException(nameof(updateAttendanceDTO));

            var attendanceToUpdate = await _attendanceRepository.GetAttendanceById(id); // Fixed: Use GetById for efficiency
            if (attendanceToUpdate == null)
                return false; // Not found, but don't throw to match return type

            _mapper.Map(updateAttendanceDTO, attendanceToUpdate);

            int rowsAffected = await _attendanceRepository.UpdateAttendance(attendanceToUpdate);
            return rowsAffected > 0;
        }
    }
}
