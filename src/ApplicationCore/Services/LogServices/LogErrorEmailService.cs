using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using ApplicationCore.Services.EmailServices;
using Microsoft.Extensions.Configuration;

namespace ApplicationCore.Services.LogServices
{
    public class LogErrorEmailService : ILogErrorService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public LogErrorEmailService(IConfiguration configuration, IEmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        public void Notify(Exception exception, string username, string urlPath)
        {
            var configurationEmail = _configuration
                .GetSection("Email:NotifyExceptionEmails")
                .GetChildren()
                .Select(x => x.Value);

            _emailService.To = new List<MailAddress>();
            foreach (var email in configurationEmail)
            {
                _emailService.To.Add(new MailAddress(email));
            }

            var siteName = _configuration["Site:SiteName"];
            _emailService.Subject = $"Se ha producido una Excepción en {siteName}";
            _emailService.From = new MailAddress(_configuration["Email:DefaultFrom"]);

            var body = new StringBuilder();
            body.Append($"Se ha producido una Excepción en {siteName}\n");
            body.Append($"Usuario: {username}\n");
            body.Append($"Url path: {urlPath}\n");
            body.Append(exception.Message);
            body.Append(exception.StackTrace);
            _emailService.Body = body.ToString();
            _emailService.SendEmailAsync().Wait();
        }
    }
}
