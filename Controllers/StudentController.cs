using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Services.Commands;
using System.Threading.Tasks;

namespace SeamsApp.Controllers
{
    [Route("api/student")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Officer")]
        [OutputCache(Duration = 30)]
        public async Task<ActionResult<IEnumerable<StudentResponse>>> GetAllActiveStudents()
        {
            var students = await _studentService.GetAllActiveStudentAsync();
            return Ok(students);
        }

        [HttpGet("{studentId:int}")]
        [Authorize(Roles = "Admin,Officer")]
        public async Task<ActionResult<StudentResponse>> GetStudentById(int studentId)
        {
            var student = await _studentService.GetStudentByIdAsync(studentId);

            if (student == null)
                return NotFound(new { Message = $"Student with ID {studentId} not found." });

            return Ok(student);
        }

        [HttpGet("qr/{schoolStudentId}")]
        [Authorize(Roles = "Admin,Officer")]
        public async Task<ActionResult<StudentResponse>> GetStudentQRCodeInfo(string schoolStudentId)
        {
            if (string.IsNullOrWhiteSpace(schoolStudentId))
                return BadRequest("School Student ID is required.");

            var student = await _studentService.GetStudentQRCodeInfoAsync(schoolStudentId);

            if (student == null)
                return NotFound(new { Message = $"Student with School ID {schoolStudentId} not found." });

            return Ok(student);
        }

        [HttpPatch("{studentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> DeleteStudent(int studentId)
        {
            var result = await _studentService.DeleteStudentByIdAsync(studentId);

            if (result == 0)
                return NotFound(new { Message = $"Student with ID {studentId} not found." });

            return Ok(new { Message = "Student deleted successfully.", DeletedId = studentId });
        }

        [HttpPut("{studentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> UpdateStudent(int studentId, [FromBody] StudentRequest studentRequest)
        {
            if (studentRequest == null)
                return BadRequest("Student data is required.");

            var result = await _studentService.UpdateStudentByIdAsync(studentId, studentRequest);

            if (result == 0)
                return NotFound(new { Message = $"Student with ID {studentId} not found." });

            return Ok(new { Message = "Student updated successfully." });
        }
    }
}