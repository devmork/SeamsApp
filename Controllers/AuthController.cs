using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Auth;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Services.Commands;
using SeamsApp.Utilities;

namespace SeamsApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IStudentApplicationService _studentApplicationService;
        public AuthController(IAuthService authService, 
               IStudentApplicationService studentApplicationService)
        {
            _authService = authService;
            _studentApplicationService = studentApplicationService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest rqs)
        {
            try
            {
                var response = await _authService.LoginAsync(rqs.Email, rqs.Password);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }

        [HttpPost("verify-email")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest rqs)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                await _studentApplicationService.VerifyEmailAsync(rqs.Email, rqs.Code);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
        }

        [HttpPost("resend-verification")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationRequest rqs)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                await _studentApplicationService.ResendVerificationAsync(rqs.Email);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
        }
    }
}
