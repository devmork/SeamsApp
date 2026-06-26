using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Event;
using SeamsApp.DTOs.Officer;
using SeamsApp.Interfaces.Services.Commands;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OfficerController : ControllerBase
    {
        private readonly IOfficerService _officerService;
        public OfficerController(IOfficerService officerService)
        {
            _officerService = officerService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(EventRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateOfficer(int userId, OfficerRequest officerRequest)
        {
            var newOfficer = await _officerService.CreateOfficerAsync(userId, officerRequest);
            if (userId == 0)
            {
                return NotFound();
            }

            return Ok(newOfficer);
        }

        [HttpPatch("{userId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> RemoveOfficer(int userId)
        { 
            var officer = await _officerService.RemoveOfficerAsync(userId);
            return Ok(officer);
        }
    }
}
