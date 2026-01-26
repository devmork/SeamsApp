using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        // GET: api/<AttendanceController>
        [HttpGet]
        public IEnumerable<string> GetAllAttendance()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AttendanceController>/5
        [HttpGet("{id}")]
        public string GetAttendanceById(int id)
        {
            return "value";
        }

        // POST api/<AttendanceController>
        [HttpPost]
        public void CreateNewAttendance([FromBody]string value)
        {
        }

        // PUT api/<AttendanceController>/5
        [HttpPut("{id}")]
        public void UpdateAttendance(int id, [FromBody]string value)
        {
        }

        // DELETE api/<AttendanceController>/5
        [HttpDelete("{id}")]
        public void DeleteAttendance(int id)
        {
        }
    }
}
