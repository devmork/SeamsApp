using FluentValidation;
using SeamsApp.DTOs.StudentApplication;

namespace SeamsApp.Validations
{
    public class CreateStudentApplicationRequestValidator
        : AbstractValidator<CreateStudentApplicationRequest>
    {
        public CreateStudentApplicationRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(256);

            RuleFor(x => x.SchoolStudentId)
                .NotEmpty().WithMessage("School Student ID is required.")
                .MinimumLength(3).MaximumLength(50);

            RuleFor(x => x.YearLevel)
                .NotEmpty().WithMessage("Year level is required.");

            RuleFor(x => x.Course)
                .NotEmpty().WithMessage("Course is required.");
        }
    }
}