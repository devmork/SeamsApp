using AutoMapper;
using SeamsApp.DTOs.Attendance;
using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Event;
using SeamsApp.DTOs.Officer;
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
            CreateMap<Student, StudentRequest>().ReverseMap();
            CreateMap<Student, StudentResponse>().ReverseMap();

            // AUTH
            CreateMap<User, UserRequest>().ReverseMap();

            // ATTENDANCE
            CreateMap<Attendance, AttendanceResponse>().ReverseMap();
            CreateMap<Attendance, AttendanceRequest>().ReverseMap();

            // STUDENT APPLICATION
            CreateMap<StudentApplication, CreateStudentApplicationRequest>().ReverseMap();
            CreateMap<StudentApplication, StudentApplicationResponse>().ReverseMap();

            // EVENT
            CreateMap<Event, EventRequest>().ReverseMap();
            CreateMap<Event, EventResponse>().ReverseMap();


            // OFFICER
            CreateMap<Officer, OfficerRequest>().ReverseMap();
            CreateMap<Officer, OfficerResponse>().ReverseMap();
        }
    }
}
