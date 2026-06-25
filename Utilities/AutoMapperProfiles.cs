using AutoMapper;
using SeamsApp.DTOs.Attendance;
using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Event;
using SeamsApp.DTOs.Student;
using SeamsApp.DTOs.StudentApplication;
using SeamsApp.Models;
using SeamsApp.Models.Base;

namespace SeamsApp.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // STUDENT
            CreateMap<Student, StudentDTO>();
            CreateMap<StudentUpdateDTO, Student>().ReverseMap();
            CreateMap<Student, CreateStudentDTO>().ReverseMap();

            // AUTH
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, CreateAdminDTO>().ReverseMap();

            // ATTENDANCE
            CreateMap<Attendance, AttendanceDTO>();
            CreateMap<Attendance, AttendanceDTO>().ReverseMap();
            CreateMap<Attendance, CreateAttendanceDTO>();
            CreateMap<Attendance, CreateAttendanceDTO>().ReverseMap();
            CreateMap<Attendance, UpdateAttendanceDTO>();
            CreateMap<Attendance, UpdateAttendanceDTO>().ReverseMap();
            CreateMap<AttendanceRecord, CreateAttendanceRecordDTO>().ReverseMap();

            CreateMap<Attendance, AttendanceResponse>().ReverseMap();
            CreateMap<Attendance, AttendanceRequest>().ReverseMap();

            // STUDENT APPLICATION
            CreateMap<StudentApplication, CreateStudentApplicationRequest>().ReverseMap();
            CreateMap<StudentApplication, StudentApplicationResponse>().ReverseMap();

            // EVENT
            CreateMap<Event, EventRequest>().ReverseMap();
            CreateMap<Event, EventResponse>().ReverseMap();

        }
    }
}
