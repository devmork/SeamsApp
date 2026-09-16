using SeamsApp.DTOs.StudentApplication;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IStudentApplicationService
    {
        Task<StudentApplicationResponse> CreateStudentApplication(StudentApplicationRequest createStudentApplicationRequest);
        Task<int> ApproveStudentApplication(int studentApplicationId);
        Task<int> RejectStudentApplication(int studentApplicationId);
        Task VerifyEmailAsync(string email, string code);
        Task ResendVerificationAsync(string email);
        Task<IEnumerable<StudentApplicationResponse>> GetAllStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllPendingStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllRejectedStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllApprovedStudentApplicationsAsync();
        
    }
}
