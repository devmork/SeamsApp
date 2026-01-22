using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SeamsApp.Data.Repositories;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services;
using SeamsApp.Models.Base;
using System.Threading.Tasks;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<int>> CreateStudent([FromBody] StudentCreationDTO studentCreationDTO)
        {
            var product = await _studentService.AddStudentAsync(studentCreationDTO);
            return Ok(product);
        }

        [HttpGet("All-Students")]
        [OutputCache]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentAsync();
            return Ok(students);
        }

        [HttpDelete("ById/{studentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> DeleteStudent(int studentId)
        {
            var student = await _studentService.DeleteStudentByIdAsync(studentId);
            return Ok(student);
        }

        [HttpGet("ById")]
        [Authorize(Roles = "Admin, Officer")]
        public async Task<ActionResult<int>> GetStudentById(int studentId)
        {
            var student = await _studentService.GetStudentByIdAsync(studentId);
            return Ok(student);
        }

        [HttpPut("ById/{studentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> UpdateStudent(StudentUpdateDTO studentUpdateDTO, int studentId)
        {
            var student = await _studentService.UpdateStudentByIdAsync(studentUpdateDTO, studentId);
            return Ok(student);
        }
    }
}
