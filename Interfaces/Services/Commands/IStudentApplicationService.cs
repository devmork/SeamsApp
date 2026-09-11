using SeamsApp.DTOs.StudentApplication;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IStudentApplicationService
    {
        Task<StudentApplicationResponse> CreateStudentApplication(CreateStudentApplicationRequest createStudentApplicationRequest);
        Task<int> ApproveStudentApplication(int studentApplicationId);
        Task<int> RejectStudentApplication(int studentApplicationId);      
        Task<IEnumerable<StudentApplicationResponse>> GetAllStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllPendingStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllRejectedStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllApprovedStudentApplicationsAsync();
        
    }
}
