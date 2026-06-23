using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Student;
using SeamsApp.DTOs.StudentApplication;
using SeamsApp.Interfaces.Services;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApplicationController : ControllerBase
    {
        private readonly IStudentApplicationService _studentApplicationService;
        public StudentApplicationController(IStudentApplicationService studentApplicationService)
        {
            _studentApplicationService = studentApplicationService;
        }

        [HttpPost("regsiter")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CreateStudentApplicationRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateStudentApplicationRequest>> CreateStudentApplication(CreateStudentApplicationRequest rqs) 
        {
            var studentApplication = await _studentApplicationService.CreateStudentApplication(rqs);
            return Ok(studentApplication);
        }

        [HttpPatch("approve-application/{studentApplicationId:int}")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StudentUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> ApproveStudentApplication(int studentApplicationId)
        {
            var result = await _studentApplicationService.ApproveStundetApplication(studentApplicationId);
            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPatch("reject-application/{studentApplicationId:int}")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StudentUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> RejectStudentApplication(int studentApplicationId)
        {
            var result = await _studentApplicationService.RejectStundetApplication(studentApplicationId);
            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }


    }
}
