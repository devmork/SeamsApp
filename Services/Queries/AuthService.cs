using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeamsApp.Data;
using SeamsApp.DTOs.Auth;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Services.Queries;
using SeamsApp.Models;
using SeamsApp.Utilities;

namespace SeamsApp.Services.Queries
{
    public class AuthService : IAuthService
    {
        private readonly SeamsDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            SeamsDbContext dbContext,
            IJwtService jwtService,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        public async Task<LoginResponseDTO> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = _jwtService.GenerateTokenAsync(user);
            var userDto = _mapper.Map<UserDTO>(user);

            return new LoginResponseDTO
            {
                Token = token,
                User = userDto
            };
        }
    }
}