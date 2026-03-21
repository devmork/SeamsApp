// Interfaces/Services/IFileUploadService.cs
using Microsoft.AspNetCore.Http;

namespace SeamsApp.Interfaces.Services
{
    public interface IFileUploadService
    {
        Task<string?> UploadStudentPhotoAsync(IFormFile? photo);
        Task<bool> DeleteStudentPhotoAsync(string? photoUrl);
    }
}