using AutoMapper;
using SeamsApp.DTOs.Attendance;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services;
using SeamsApp.Models;
using System;

namespace SeamsApp.Services
{
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private readonly IAttendanceRecordRepository _attendanceRecordRepository;
        private readonly IMapper _mapper;
        
        public AttendanceRecordService
        (
            IAttendanceRecordRepository attendanceRecordRepository,
            IMapper mapper
        )
        {
            _attendanceRecordRepository = attendanceRecordRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAttendanceRecordAsync(CreateAttendanceRecordDTO attendanceRecordDTO)
        {
            if (attendanceRecordDTO == null)
                throw new ArgumentNullException(nameof(attendanceRecordDTO));

            // Check for duplicate attendance
            bool isDuplicate = await _attendanceRecordRepository.CheckDuplicateAttendance(
                attendanceRecordDTO.AttendanceID, 
                attendanceRecordDTO.SchoolStudentID);

            var attendanceRecord = _mapper.Map<AttendanceRecord>(attendanceRecordDTO);

            if (isDuplicate)
            {
                throw new InvalidOperationException($"Student {attendanceRecordDTO.SchoolStudentID} has already been marked for attendance ID {attendanceRecord.AttendanceID} today.");
            }

            int newId = await _attendanceRecordRepository.RecordStudentAttendance(attendanceRecord);
            return newId;
        }

        public async Task<List<AttendanceRecordDTO>> GetListOfAttendanceRecordByAttendanceEventName(
            string attendanceEventName,
            string logType,
            int semester,
            int year
        )
        {
            var attendanceRecords = await _attendanceRecordRepository.GetListOfAttendanceRecordByAttendanceEventName
            (
                attendanceEventName,
                logType,
                semester,
                year
            );
            return attendanceRecords;
        }
    }
}