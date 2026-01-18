using AutoMapper;
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
            CreateMap<StudentCreationDTO, Student>();
            CreateMap<StudentUpdateDTO, Student>();


            // Auth
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();
        }
    }
}
