using AutoMapper;
using SeamsApp.DTOs.Attendance;
using SeamsApp.DTOs.AttendanceRecords;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;
using System;

namespace SeamsApp.Services.Commands
{
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private readonly IMapper _mapper;
        
        public AttendanceRecordService
        (
            IMapper mapper
        )
        {
            _mapper = mapper;
        }

    }
}