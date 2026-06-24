using SeamsApp.DTOs.StudentApplication;

namespace SeamsApp.Interfaces.Services
{
    public interface IStudentApplicationService
    {
        Task<CreateStudentApplicationRequest> CreateStudentApplication(CreateStudentApplicationRequest createStudentApplicationRequest);
        Task<int> ApproveStundetApplication(int studentApplicationId);
        Task<int> RejectStudentApplication(int studentApplicationId);      
        Task<IEnumerable<StudentApplicationResponse>> GetAllStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllPendingStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllRejectedStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllApprovedStudentApplicationsAsync();
        
    }
}
