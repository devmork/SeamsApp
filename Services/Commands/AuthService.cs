using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeamsApp.Data;
using SeamsApp.DTOs.Auth;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;
using SeamsApp.Utilities;

namespace SeamsApp.Services.Commands
{
    public class AuthService : IAuthService
    {
        private readonly SeamsDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(SeamsDbContext dbContext,
                           IPasswordHasher<User> passwordHasher,
                           IMapper mapper,
                           IJwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email) ?? 
                throw new UnauthorizedAccessException("Invalid email or password.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password.");

            var token = _jwtService.GenerateToken(user);

            return new LoginResponse
            {
                Token = token,
                UserId = user.UserId,
                Email = user.Email!,
                Role = user.Role!
            };
        }
    }
}