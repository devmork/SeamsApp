using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Attendance;
using SeamsApp.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // GET: api/attendance
        // Returns list of active attendance records
        [HttpGet]
        [ProducesResponseType(typeof(List<AttendanceDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<AttendanceDTO>>> GetAllAttendance()
        {
            var attendanceList = await _attendanceService.GetAllAttendanceAsync();
            return Ok(attendanceList);
        }

        // GET: api/attendance/5
        // Returns attendance record by ID
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

        // POST: api/attendance
        // Added for completeness: Creates a new attendance
        [HttpPost]
        [ProducesResponseType(typeof(int), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<int>> CreateAttendance([FromBody] CreateAttendanceDTO createAttendanceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newId = await _attendanceService.CreateAttendanceAsync(createAttendanceDTO);
            return CreatedAtAction(nameof(GetAttendanceById), new { id = newId }, newId);
        }

        // PUT: api/attendance/5
        // Added for completeness: Updates an attendance
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

        // DELETE: api/attendance/5
        // Added for completeness: Deletes (soft) an attendance
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

        //// POST: api/attendance/5/record/10
        //// Added for completeness: Records student attendance
        //[HttpPost("{attendanceId}/record/{studentId}")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(409)] // Conflict if duplicate
        //[ProducesResponseType(500)]
        //public async Task<ActionResult<bool>> RecordStudentAttendance(int attendanceId, int studentId)
        //{
        //    var success = await _attendanceService.RecordStudentAttendance(attendanceId, studentId);
        //    if (!success)
        //    {
        //        return Conflict("Attendance already recorded for this student.");
        //    }
        //    return Ok(true);
        //}
    }
}
