using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.AttendanceRecords;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Utilities;

namespace SeamsApp.Controllers
{
    [Route("api/attendance-record")]
    [ApiController]
    [Authorize]
    public class AttendanceRecordController : ControllerBase
    {
        private readonly IAttendanceRecordService _attendanceRecordService;

        public AttendanceRecordController(IAttendanceRecordService attendanceRecordService)
        {
            _attendanceRecordService = attendanceRecordService;
        }

        [Authorize(Roles = "Admin, Officer")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AttendanceRecordResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AttendanceRecordResponse>>> GetAllAttendanceRecords()
        {
            var records = await _attendanceRecordService.GetAllAttendanceRecordsAsync();
            return Ok(records);
        }

        [Authorize(Roles = "Admin, Officer, Student")]
        [HttpGet("{recordId:int}")]
        [ProducesResponseType(typeof(AttendanceRecordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceRecordResponse>> GetAttendanceRecordById(int recordId)
        {
            var record = await _attendanceRecordService.GetAttendanceRecordByIdAsync(recordId);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [Authorize(Roles = "Admin, Officer")]
        [HttpGet("attendance/{attendanceId:int}")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceRecordResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AttendanceRecordResponse>>> GetAttendanceRecordsByAttendanceId(int attendanceId)
        {
            var records = await _attendanceRecordService.GetAttendanceRecordsByAttendanceIdAsync(attendanceId);
            if (records == null || !records.Any())
            {
                return NotFound($"No attendance records found for attendance ID: {attendanceId}");
            }
            return Ok(records);
        }

        [Authorize(Roles = "Admin, Officer")]
        [HttpPost]
        [ProducesResponseType(typeof(AttendanceRecordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceRecordResponse>> CreateAttendanceRecord([FromBody] AttendanceRecordRequest attendanceRecordRequest)
        {
            var newRecord = await _attendanceRecordService.CreateAttendanceRecordAsync(attendanceRecordRequest);
            if (newRecord == null)
            {
                return BadRequest("Unable to create attendance record. The attendance session may not exist, or this student was already logged.");
            }
            return Ok(newRecord);
        }

        [Authorize(Roles = "Admin, Officer")]
        [HttpPut("{recordId:int}")]
        [ProducesResponseType(typeof(AttendanceRecordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceRecordResponse>> UpdateAttendanceRecord(int recordId, [FromBody] AttendanceRecordRequest attendanceRecordRequest)
        {
            var record = await _attendanceRecordService.UpdateAttendanceRecordAsync(recordId, attendanceRecordRequest);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{recordId:int}")]
        [ProducesResponseType(typeof(AttendanceRecordResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AttendanceRecordResponse>> DeleteAttendanceRecord(int recordId)
        {
            var record = await _attendanceRecordService.DeleteAttendanceRecordAsync(recordId);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("me")]
        [ProducesResponseType(typeof(IEnumerable<EventAttendanceGroupResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EventAttendanceGroupResponse>>> GetMyAttendanceHistory()
        {
            var userId = ClaimsUtility.GetUserIdFromClaims(HttpContext);
            var history = await _attendanceRecordService.GetStudentAttendanceHistoryAsync(userId);
            return Ok(history);
        }
    }
}