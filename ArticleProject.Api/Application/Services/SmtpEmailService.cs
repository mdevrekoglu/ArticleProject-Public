using ArticleProject.Domain.Common.Interfaces.Service;
using ArticleProject.Model.Models.CommonModels.DataTransferModels;
using System.Net;
using System.Net.Mail;

namespace ArticleProject.Api.Application.Services
{
    public class SmtpEmailService : IBaseSingletonService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly string _frontEndUrl;

        public SmtpEmailService(IConfiguration configuration)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettingsModel>();
            _fromEmail = smtpSettings.Email; // Sender mail
            _smtpClient = new SmtpClient(smtpSettings.Server)
            {
                Port = smtpSettings.Port,
                Credentials = new NetworkCredential(smtpSettings.Email, smtpSettings.Password),
                EnableSsl = smtpSettings.EnableSsl
            };
            _frontEndUrl = configuration.GetValue<string>("FrontendSettings:BaseUrl"); ;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body);
            mailMessage.IsBodyHtml = true;  // If in html format

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public string GenerateCallbackUri(string path, string query)
        {
            // Construct the full URL for HashRouter
            var hashPath = $"{path}?{query}"; // This will be appended after the hash

            // Ensure your base URL ends correctly
            var baseUrl = _frontEndUrl.TrimEnd('/');

            // Final URL with hash
            var finalUrl = $"{baseUrl}/#/{hashPath.TrimStart('/')}";

            return finalUrl;
        }
    }
}