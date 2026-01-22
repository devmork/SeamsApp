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

/// <summary>
        /// 
        /// </summary>
        /// <param name="studentCreationDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Admin")]
[ProducesResponseType(typeof(StudentCreationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateStudent([FromBody] StudentCreationDTO studentCreationDTO)
        {
            var product = await _studentService.AddStudentAsync(studentCreationDTO);
            return Ok(product);
        }

/// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("All-Students")]
[Authorize(Roles = "Admin, Officer")]
        [OutputCache]
[ProducesResponseType(typeof(IEnumerable<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentAsync();
            return Ok(students);
        }

/// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpDelete("ById/{studentId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> DeleteStudent(int studentId)
        {
            var student = await _studentService.DeleteStudentByIdAsync(studentId);
            return Ok(student);
        }

/// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("ById")]
        [Authorize(Roles = "Admin, Officer")]
[ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
