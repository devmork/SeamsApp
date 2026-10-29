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
            CreateMap<Student, StudentResponse>()
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
                    .ReverseMap();

            // AUTH
            CreateMap<User, UserRequest>().ReverseMap();

            // ATTENDANCE
            CreateMap<Attendance, AttendanceResponse>().ReverseMap();
            CreateMap<Attendance, AttendanceRequest>().ReverseMap();

            // STUDENT APPLICATION
            CreateMap<StudentApplication, StudentApplicationRequest>().ReverseMap();
            CreateMap<StudentApplication, StudentApplicationResponse>().ReverseMap();

            // EVENT
            CreateMap<Event, EventRequest>()
                .ForMember(dest => dest.TargetYearLevels, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.TargetYearLevels)
                        ? new List<string>()
                        : src.TargetYearLevels
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToList()));

            CreateMap<EventRequest, Event>()
                .ForMember(dest => dest.TargetYearLevels, opt => opt.MapFrom(src =>
                    src.TargetYearLevels != null && src.TargetYearLevels.Any()
                        ? string.Join(',', src.TargetYearLevels)
                        : null));

            CreateMap<Event, EventResponse>()
                .ForMember(dest => dest.TargetYearLevels, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.TargetYearLevels)
                        ? new List<string>()
                        : src.TargetYearLevels
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToList()));

            // OFFICER
            CreateMap<Officer, OfficerRequest>().ReverseMap();
            CreateMap<Officer, OfficerResponse>().ReverseMap();
        }
    }
}
