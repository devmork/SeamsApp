using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services;
using SeamsApp.Models;

namespace SeamsApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(IUserRepository userRepository,
                            IJwtService jwtService,
                            IMapper mapper,
                            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        public async Task<LoginResponseDTO> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = await _jwtService.GenerateTokenAsync(user);
            var userDto = _mapper.Map<UserDTO>(user);

            return new LoginResponseDTO
            {
                Token = token,
                User = userDto
            };
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return _mapper.Map<UserDTO>(user);
        }
    }
}
