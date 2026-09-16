using SeamsApp.Validations;
using System.ComponentModel.DataAnnotations;

namespace SeamsApp.DTOs.StudentApplication
{
    public class CheckApplicationAvailabilityResponse
    {
        public bool EmailRegistered { get; set; }
        public bool EmailPending { get; set; }
        public bool SchoolStudentIdTaken { get; set; }
        public bool EmailTaken => EmailRegistered || EmailPending;
        public bool IsAvailable => !EmailTaken && !SchoolStudentIdTaken;
    }
}
