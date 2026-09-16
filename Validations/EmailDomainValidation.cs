using System.ComponentModel.DataAnnotations;

namespace SeamsApp.Validations
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public sealed class EmailDomainValidation : ValidationAttribute
    {
        public const string AllowedDomain = "dmc.edu.ph";

        public EmailDomainValidation()
        {
            ErrorMessage = $"Email must be a valid @{AllowedDomain} address.";
        }

        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            // Let [Required] handle null/empty — don't double-report.
            if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;

            var email = value.ToString()!.Trim();
            var atIndex = email.LastIndexOf('@');

            // Must have a local part and a domain part.
            if (atIndex < 1 || atIndex == email.Length - 1)
                return new ValidationResult(ErrorMessage);

            var domain = email[(atIndex + 1)..];

            return string.Equals(domain, AllowedDomain, StringComparison.OrdinalIgnoreCase)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage);
        }
    }
}
