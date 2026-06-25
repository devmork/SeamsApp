using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services.Queries;
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
        /// <param name="createStudentDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateStudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateStudentDTO>> CreateStudent(CreateStudentDTO createStudentDTO)
        {
            var student = await _studentService.CreateStudent(createStudentDTO);
            return Ok(student);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("PendingStudents")]
        [Authorize(Roles = "Admin, Officer")]
        [OutputCache]
        [ProducesResponseType(typeof(IEnumerable<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetPendingStudents()
        {
            var students = await _studentService.GetAllPendingStudentAsync();
            return Ok(students);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ApprovedStudents")]
        [Authorize(Roles = "Admin, Officer")]
        [OutputCache]
        [ProducesResponseType(typeof(IEnumerable<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetApprovedStudents()
        {
            var students = await _studentService.GetAllApprovedStudentAsync();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentUpdateDTO"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpPut("ById/{studentId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(StudentUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> UpdateStudent(StudentUpdateDTO studentUpdateDTO, int studentId)
        {
            var student = await _studentService.UpdateStudentByIdAsync(studentUpdateDTO, studentId);
            return Ok(student);
        }

        /// <summary>
        /// Approve a pending student registration
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpPut("Approve/{studentId:int}")]
        [Authorize(Roles = "Admin, Officer")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApproveStudent(int studentId)
        {
            try
            {
                var student = await _studentService.ApprovedStudentAsync(studentId);
                return Ok(new { Message = "Student approved successfully", Student = student });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Reject a pending student registration
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpPut("Reject/{studentId:int}")]
        [Authorize(Roles = "Admin, Officer")]
        [ProducesResponseType(typeof(StudentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RejectStudent(int studentId)
        {
            try
            {
                var student = await _studentService.RejectStudentAsync(studentId);
                return Ok(new { Message = "Student rejected", Student = student });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

    }
}
