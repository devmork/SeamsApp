using AutoMapper;
using SeamsApp.DTOs.Attendance;
using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Student;
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


            // Auth
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, RegisterUserDTO>().ReverseMap();

            //Attendance
            CreateMap<Attendance, AttendanceDTO>();
            CreateMap<Attendance, AttendanceDTO>().ReverseMap();
            CreateMap<Attendance, CreateAttendanceDTO>();
            CreateMap<Attendance, CreateAttendanceDTO>().ReverseMap();
            CreateMap<Attendance, UpdateAttendanceDTO>();
            CreateMap<Attendance, UpdateAttendanceDTO>().ReverseMap();

        }
    }
}
