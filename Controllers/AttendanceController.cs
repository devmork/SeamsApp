using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Attendance;
using SeamsApp.Interfaces.Services;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Officer, Student")]
        [HttpGet]
        [ProducesResponseType(typeof(List<AttendanceDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<AttendanceDTO>>> GetAllAttendance()
        {
            var attendanceList = await _attendanceService.GetAllAttendanceAsync();
            return Ok(attendanceList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Officer")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AttendanceDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<AttendanceDTO>> GetAttendanceById(int id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return Ok(attendance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createAttendanceDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("create-attendance")]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<int>> CreateAttendance([FromBody] CreateAttendanceDTO createAttendanceDTO)
        {
            try
            {
                var newId = await _attendanceService.CreateAttendanceAsync(createAttendanceDTO);
                return Ok(newId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateAttendanceDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAttendance(int id, [FromBody] UpdateAttendanceDTO updateAttendanceDTO)
        {
            var success = await _attendanceService.UpdateAttendanceAsync(id, updateAttendanceDTO);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAttendance(int id)
        {
            var success = await _attendanceService.DeleteAttendanceAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }        
    }
}
