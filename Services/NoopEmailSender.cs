using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ServiceLink.Services
{
    // Dev-friendly no-op email sender (does nothing)
    public class NoopEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Optionally: log to console for debugging
            System.Diagnostics.Debug.WriteLine($"[NoopEmailSender] To:{email} Subject:{subject}");
            return Task.CompletedTask;
        }
    }
}
