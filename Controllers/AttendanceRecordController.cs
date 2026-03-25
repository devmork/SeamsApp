using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Attendance;
using SeamsApp.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceRecordController : ControllerBase
    {
        private readonly IAttendanceRecordService _attendanceRecordService;
        public AttendanceRecordController
        (
            IAttendanceRecordService attendanceRecordService
        )
        {
            _attendanceRecordService = attendanceRecordService;
        }
        // // GET: api/<AttendanceRecord>
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }

        // // GET api/<AttendanceRecord>/5
        // [HttpGet("{id}")]
        // public string Get(int id)
        // {
        //     return "value";
        // }

        // // POST api/<AttendanceRecord>
        // [HttpPost]
        // public void Post([FromBody]string value)
        // {
        // }

        // // PUT api/<AttendanceRecord>/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody]string value)
        // {
        // }

        // // DELETE api/<AttendanceRecord>/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }

        [HttpPost("record-attendance")]
        [Authorize(Roles = "Officer")]
        public async Task<ActionResult> RecordStudentAttendance([FromBody] CreateAttendanceRecordDTO createAttendanceRecordDTO) 
        {
            await _attendanceRecordService.CreateAttendanceRecordAsync(createAttendanceRecordDTO);
            return Ok("Successfully recording of attendance");
        }

        [HttpGet("attendance-record-list")]
        [Authorize(Roles = "Admin, Officer, Student")]
        public async Task<ActionResult> GetListOfAttendanceRecordByAttendanceEventName
        (
            string attendanceEventName,
            string logType,
            int semester,
            int year
        )
        {
            var attendanceRecordsList = await _attendanceRecordService.GetListOfAttendanceRecordByAttendanceEventName
            (
                attendanceEventName,
                logType,
                semester,
                year
            );
            return Ok(attendanceRecordsList);
        }
    }
}
