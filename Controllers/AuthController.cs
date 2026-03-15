using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Auth;
using SeamsApp.Interfaces.Services;
using SeamsApp.Utilities;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService service)
        {
            _authService = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
        {
            try
            {
                var user = await _authService.CreateUserAsync(dto);
                return Ok(new { Message = "User registered successfully", User = user });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            try
            {
                var response = await _authService.LoginAsync(dto.Email, dto.Password);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("All Users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers() 
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Id")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserById()
        {
            var userId = ClaimsUtility.GetUserIdFromClaims(HttpContext);
            var user = await _authService.GetUserByIdAsync(userId);
            return Ok(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Email")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserByEmail()
        {
            var userEmail = ClaimsUtility.GetUserEmailFromClaims(HttpContext);
            var user = await _authService.GetUserByEmailAsync(userEmail!);
            return Ok(user);
        }

        /// <summary>
        /// Assigns role to a user (e.g., Admin, User). Only accessible by Admins.
        /// </summary>
        /// <returns></returns>
        [HttpPost("AssignRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO dto)
        {
            try
            {
                await _authService.AssignRoleAsync(dto.UserId, dto.RoleId);
                return Ok(new { Message = "Role has been assigned to the user successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
