using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeamsApp.Interfaces.Services;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(
            IFileUploadService fileUploadService,
            ILogger<FileUploadController> logger)
        {
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        /// <summary>
        /// Upload a student photo and return the URL
        /// </summary>
        /// <param name="photo">The image file to upload</param>
        /// <returns>The URL of the uploaded photo</returns>
        [HttpPost("StudentPhoto")]
        [AllowAnonymous] // Change to [Authorize] if you want to restrict access
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UploadStudentPhoto(IFormFile photo)
        {
            // Validate that a file was provided
            if (photo == null || photo.Length == 0)
            {
                return BadRequest(new { error = "No photo file provided" });
            }

            // Validate file size (max 5MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5MB
            if (photo.Length > maxFileSize)
            {
                return BadRequest(new { error = $"File size cannot exceed {maxFileSize / 1024 / 1024}MB" });
            }

            // Validate file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { error = $"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}" });
            }

            // Upload the file
            var photoUrl = await _fileUploadService.UploadStudentPhotoAsync(photo);
            
            if (photoUrl == null)
            {
                _logger.LogError("Failed to upload photo: {FileName}", photo.FileName);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "Failed to upload photo" });
            }

            _logger.LogInformation("Photo uploaded successfully: {PhotoUrl}", photoUrl);
            return Ok(new { photoUrl, message = "Photo uploaded successfully" });
        }

        /// <summary>
        /// Delete a student photo
        /// </summary>
        /// <param name="photoUrl">The URL of the photo to delete</param>
        [HttpDelete("StudentPhoto")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteStudentPhoto([FromQuery] string photoUrl)
        {
            if (string.IsNullOrEmpty(photoUrl))
            {
                return BadRequest(new { error = "Photo URL is required" });
            }

            var result = await _fileUploadService.DeleteStudentPhotoAsync(photoUrl);
            
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { error = "Failed to delete photo" });
            }

            return Ok(new { message = "Photo deleted successfully" });
        }
    }
}