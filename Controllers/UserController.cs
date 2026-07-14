using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Auth;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Utilities;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<IEnumerable<UserRequest>>> GetAllUsers()
        //{
        //    var users = await _userService.GetAllUsersAsync();
        //    return Ok(users);
        //}

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<IEnumerable<UserRequest>>> GetUserById()
        //{
        //    var userId = ClaimsUtility.GetUserIdFromClaims(HttpContext);
        //    var user = await _userService.GetUserByIdAsync(userId);
        //    return Ok(user);
        //}

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<IEnumerable<UserRequest>>> GetUserByEmail()
        //{
        //    var userEmail = ClaimsUtility.GetUserEmailFromClaims(HttpContext);
        //    var user = await _userService.GetUserByEmailAsync(userEmail!);
        //    return Ok(user);
        //}

    }
}
