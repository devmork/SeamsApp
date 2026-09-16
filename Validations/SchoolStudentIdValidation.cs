using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SeamsApp.Validations
{
    /// <summary>
    /// Validates that a School Student ID matches the DMC format
    /// <c>0000-0000</c> (four digits, hyphen, four digits).
    /// Example: <c>2023-0444</c>.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public sealed class SchoolStudentIdValidation : ValidationAttribute
    {
        // Compiled once, reused for every validation call.
        private static readonly Regex FormatPattern =
            new(@"^\d{4}-\d{4}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public SchoolStudentIdValidation()
        {
            ErrorMessage = "School Student ID must follow the format 0000-0000 (e.g., 2023-0444).";
        }

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            // Let [Required] handle null/empty.
            if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            var id = value.ToString()!.Trim();

            return FormatPattern.IsMatch(id)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}