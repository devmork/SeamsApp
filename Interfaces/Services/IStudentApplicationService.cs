using SeamsApp.DTOs.StudentApplication;

namespace SeamsApp.Interfaces.Services
{
    public interface IStudentApplicationService
    {
        Task<CreateStudentApplicationRequest> CreateStudentApplication(CreateStudentApplicationRequest createStudentApplicationRequest);
        Task<int> ApproveStundetApplication(int studentApplicationId);
        Task<int> RejectStundetApplication(int studentApplicationId);      
        Task<IEnumerable<StudentApplicationResponse>> GetAllStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllPendingStudentApplicationsAsync();
        Task<IEnumerable<StudentApplicationResponse>> GetAllApproveStudentApplicationsAsync();
        
    }
}
