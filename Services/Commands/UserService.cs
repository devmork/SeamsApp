using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;

namespace SeamsApp.Services.Commands
{
    public class UserService : IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public UserService(IJwtService jwtService,
                           IMapper mapper)
        {
            _jwtService = jwtService;
            _mapper = mapper;
        }
        public Task<IEnumerable<UserRequest>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserRequest> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserRequest> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
