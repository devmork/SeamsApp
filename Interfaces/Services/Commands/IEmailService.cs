namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string toEmail, string otpCode);
        Task SendApplicationApprovedEmailAsync(string toEmail, string firstName, string tempPassword);
    }
}
