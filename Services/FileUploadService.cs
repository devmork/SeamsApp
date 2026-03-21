// Services/FileUploadService.cs
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SeamsApp.Interfaces.Services;

namespace SeamsApp.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileUploadService> _logger;
        private readonly IConfiguration _configuration;

        public FileUploadService(
            IWebHostEnvironment environment,
            ILogger<FileUploadService> logger,
            IConfiguration configuration) // ✅ Just inject IConfiguration
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string?> UploadStudentPhotoAsync(IFormFile? photo)
        {
            // If no photo provided, return null
            if (photo == null || photo.Length == 0)
            {
                return null;
            }

            // ✅ Get settings directly from configuration (with defaults)
            var maxFileSizeMB = _configuration.GetValue<int>("FileUpload:MaxFileSizeMB", 5);
            var maxFileSizeBytes = maxFileSizeMB * 1024 * 1024;
            var allowedExtensions = _configuration.GetSection("FileUpload:AllowedExtensions").Get<string[]>() 
                ?? new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var uploadPathConfig = _configuration["FileUpload:StudentPhotoPath"] ?? "uploads/students";

            // Validate file size
            if (photo.Length > maxFileSizeBytes)
            {
                _logger.LogWarning("File {FileName} exceeds {MaxSize}MB limit", photo.FileName, maxFileSizeMB);
                return null;
            }

            // Validate file extension
            var fileExtension = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                _logger.LogWarning("File {FileName} has invalid extension: {Extension}. Allowed: {Allowed}", 
                    photo.FileName, fileExtension, string.Join(", ", allowedExtensions));
                return null;
            }

            try
            {
                // Define upload path
                var webRootPath = _environment.WebRootPath ?? _environment.ContentRootPath;
                var uploadPath = Path.Combine(webRootPath, uploadPathConfig);

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                    _logger.LogInformation("Created upload directory: {UploadPath}", uploadPath);
                }

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadPath, uniqueFileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream);
                }

                _logger.LogInformation("Photo uploaded: {FileName} -> {UniqueFileName}", photo.FileName, uniqueFileName);

                // Return URL path
                return $"/{uploadPathConfig}/{uniqueFileName}".Replace("\\", "/");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading photo for student: {FileName}", photo.FileName);
                return null;
            }
        }

        public async Task<bool> DeleteStudentPhotoAsync(string? photoUrl)
        {
            if (string.IsNullOrEmpty(photoUrl))
            {
                return true;
            }

            try
            {
                var webRootPath = _environment.WebRootPath ?? _environment.ContentRootPath;
                var relativePath = photoUrl.TrimStart('/');
                var filePath = Path.Combine(webRootPath, relativePath);

                if (File.Exists(filePath))
                {
                    await Task.Run(() => File.Delete(filePath));
                    _logger.LogInformation("Deleted photo: {PhotoUrl}", photoUrl);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting photo: {PhotoUrl}", photoUrl);
                return false;
            }
        }
    }
}