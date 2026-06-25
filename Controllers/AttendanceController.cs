using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Attendance;
using SeamsApp.DTOs.Event;
using SeamsApp.Interfaces.Services.Commands;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendaceService _attendanceService;

        public AttendanceController(IAttendaceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [Authorize(Roles = "Admin, Officer, Student")]
        [HttpGet]
        [ProducesResponseType(typeof(AttendanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AttendanceResponse>>> GetAllAttendance()
        {
            var attendances = await _attendanceService.GetAllAttendanceAsync();
            if (attendances == null)
            {
                return NotFound();
            }
            return Ok(attendances);
        }

        [Authorize(Roles = "Admin, Officer")]
        [HttpGet("{attendanceId:int}")]
        [ProducesResponseType(typeof(AttendanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceResponse>> GetAttendanceById(int attendanceId)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(attendanceId);
            if (attendance == null)
            {
                return NotFound();
            }
            return Ok(attendance);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{eventId:int}/attendances")]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceRequest>> CreateAttendance(int eventId, [FromBody] AttendanceRequest attendanceRequest)
        {
            var newAttendance = await _attendanceService.CreateAttendanceAsync(eventId, attendanceRequest);
            if (newAttendance == null)
            {
                return BadRequest();
            }

            return Ok(newAttendance);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{attendanceId:int}")]
        [ProducesResponseType(typeof(AttendanceRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceRequest>> UpdateAttendance(int attendanceId, [FromBody] AttendanceRequest attendanceRequest)
        {
            var attendance = await _attendanceService.UpdateAttendanceAsync(attendanceId, attendanceRequest);
            if (attendance == null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{attendanceId:int}")]
        [ProducesResponseType(typeof(AttendanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceResponse>> DeleteAttendance(int attendanceId)
        {
            var attendance = await _attendanceService.DeleteAttendanceAsync(attendanceId);
            if (attendance == null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }
    }
}
