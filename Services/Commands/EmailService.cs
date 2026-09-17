
using Resend;
using SeamsApp.Interfaces.Services.Commands;

namespace SeamsApp.Services.Commands
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly IConfiguration _config;

        public EmailService(IResend resend, IConfiguration config)
        {
            _resend = resend;
            _config = config;
        }
        public async Task SendApplicationApprovedEmailAsync(string toEmail, string firstName, string tempPassword)
        {
            var message = new EmailMessage
            {
                From = _config["Resend:FromEmail"]!,
                Subject = "Your SEAMS application has been approved",
                HtmlBody = $"""
                    <p>Hi {firstName},</p>
                    <p>Your student application has been approved. You can now log in with:</p>
                    <p><strong>Email:</strong> {toEmail}<br/>
                    <strong>Temporary password:</strong> {tempPassword}</p>
                    <p>Please log in and change your password as soon as possible.</p>
                    """
            };
            message.To.Add(toEmail);

            var response = await _resend.EmailSendAsync(message);
            if (!response.Success)
            {
                throw new InvalidOperationException(
                    $"Failed to send verification email: {response.Exception?.Message ?? "unknown error"}");
            }
        }

        public async Task SendOtpEmailAsync(string toEmail, string otpCode)
        {
            var message = new EmailMessage
            {
                From = _config["Resend:FromEmail"]!,
                Subject = "Verify your SEAMS account",
                HtmlBody = $"""
                    <p>Your verification code is:</p>
                    <h2 style="letter-spacing:4px">{otpCode}</h2>
                    <p>This code expires in 10 minutes. If you didn't request this, ignore this email.</p>
                    """
            };
            message.To.Add(toEmail);

            var response = await _resend.EmailSendAsync(message);
            if (!response.Success)
            {
                throw new InvalidOperationException(
                    $"Failed to send verification email: {response.Exception?.Message ?? "unknown error"}");
            }
        }
    }
}
