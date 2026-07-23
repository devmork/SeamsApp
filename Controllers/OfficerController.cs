using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Event;
using SeamsApp.DTOs.Officer;
using SeamsApp.Interfaces.Services.Commands;

namespace SeamsApp.Controllers
{
    [Route("api/officer")]
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
        public async Task<ActionResult<int>> CreateOfficer(int userId)
        {
            var newOfficer = await _officerService.CreateOfficerAsync(userId);
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<OfficerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OfficerResponse>>> GetAllOfficers()
        {
            try
            {
                var officers = await _officerService.GetAllOfficers();
                return Ok(officers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while retrieving officers.", Error = ex.Message });
            }
        }
    }
}
