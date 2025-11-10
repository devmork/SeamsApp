using Microsoft.AspNetCore.Mvc;
using SeamsApp.Data.Repositories;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models.Base;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentsRepository _studentRepository;
        public StudentController(IStudentsRepository studentsRepository)
        {
            _studentRepository = studentsRepository;
        }
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await _studentRepository.GetAllStudent();
            if (students == null)
            {
                return BadRequest();
            }
            return Ok(students);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> AddStudent([FromBody] Student student)
        {
            return Ok(await _studentRepository.AddStudent(student));
        }

        [HttpPut("{id}")]
        public void UpdateStudent(int id, [FromBody] Student student)
        {
        }

        [HttpGet]
        public async Task<ActionResult<Student>> GetStudentByID(string schoolStudentID )
        {
            var student = await _studentRepository.GetStudentById(schoolStudentID);

            if (student == null)
            {
                return BadRequest();
            }

            return Ok(student);
        }

        [HttpGet]
        public async Task<ActionResult<Student>> GetStudentQRCode(string schoolStudentID)
        {
            var qrcode = await _studentRepository.GetStudentQRCode(schoolStudentID);

            if (qrcode == null)
            {
                return BadRequest();
            }

            return Ok(qrcode);
        }
    }
}
