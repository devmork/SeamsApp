using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.DTOs.Student;
using SeamsApp.DTOs.StudentApplication;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;

namespace SeamsApp.Controllers
{
    [Route("api/student-application")]
    [ApiController]
    public class StudentApplicationController : ControllerBase
    {
        private readonly IStudentApplicationService _studentApplicationService;

        public StudentApplicationController(IStudentApplicationService studentApplicationService)
        {
            _studentApplicationService = studentApplicationService;
        }

        [HttpPost("signup")]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> RejectStudentApplication(int studentApplicationId)
        {
            var result = await _studentApplicationService.RejectStudentApplication(studentApplicationId);
            if (result == 0)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("all-applications")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(StudentApplicationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentApplicationResponse>>> GetAllStudentApplications()
        {
            var allApplications = await _studentApplicationService.GetAllStudentApplicationsAsync();
            if (allApplications == null)
            {
                return NotFound();
            }
            return Ok(allApplications);
        }



        [HttpGet("approved-applications")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(StudentApplicationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentApplicationResponse>>> GetAllApprovedStudentApplications()
        {
            var approvedApplications = await _studentApplicationService.GetAllApprovedStudentApplicationsAsync();
            if (approvedApplications == null)
            {
                return NotFound();
            }
            return Ok(approvedApplications);
        }


        [HttpGet("rejected-applications")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(StudentApplicationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentApplicationResponse>>> GetAllRejectedStudentApplications()
        {
            var rejectedApplications = await _studentApplicationService.GetAllRejectedStudentApplicationsAsync();
            if (rejectedApplications == null)
            {
                return NotFound();
            }

            return Ok(rejectedApplications);
        }

        [HttpGet("pending-applications")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(StudentApplicationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentApplicationResponse>>> GetAllPendingStudentApplications()
        {
            var pendingApplications = await _studentApplicationService.GetAllPendingStudentApplicationsAsync();
            if (pendingApplications == null)
            {
                return NotFound();
            }

            return Ok(pendingApplications);
        }



    }
}
