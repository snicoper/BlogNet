using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Services.ViewServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApplicationCore.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IViewRenderService _viewRenderService;
        private readonly ILogEmailRepository _logEmailRepository;

        public EmailService(
            IConfiguration configuration,
            ILogger<EmailService> logger,
            IViewRenderService viewRenderService,
            ILogEmailRepository logEmailRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _viewRenderService = viewRenderService;
            _logEmailRepository = logEmailRepository;
        }

        public bool IsHtml { get; set; } = false;
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<MailAddress> To { get; set; }
        public MailAddress From { get; set; }
        public bool RegisterEmail { get; set; }

        public async Task SendEmailAsync(string viewName, object model)
        {
            Body = await _viewRenderService.RenderToStringAsync(viewName, model);
            await SendEmailAsync();
        }

        public async Task SendEmailAsync()
        {
            _validate();

            using (var smtpClient = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _configuration["Email:Smtp:Username"],
                    Password = _configuration["Email:Smtp:Password"]
                };

                smtpClient.Credentials = credential;
                smtpClient.Host = _configuration["Email:Smtp:Host"];
                smtpClient.Port = int.Parse(_configuration["Email:Smtp:Port"]);
                smtpClient.EnableSsl = bool.Parse(_configuration["Email:Smtp:EnableSsl"]);

                using (var emailMessage = new MailMessage())
                {
                    foreach (var emailTo in To)
                    {
                        emailMessage.To.Add(emailTo);
                    }

                    emailMessage.IsBodyHtml = IsHtml;
                    emailMessage.From = From;
                    emailMessage.Subject = Subject;
                    emailMessage.Body = Body;

                    var sendMails = bool.Parse(_configuration["Email:SendMails"]);
                    if (sendMails)
                    {
                        await smtpClient.SendMailAsync(emailMessage);
                    }
                    else
                    {
                        _loggerEmail();
                    }
                }
            }

            if (RegisterEmail)
            {
                await _registerLogEmail();
            }
        }

        private void _validate()
        {
            if (string.IsNullOrEmpty(Subject))
            {
                throw new ArgumentNullException("", "El campo Subject es requerido");
            }

            if (string.IsNullOrEmpty(Body))
            {
                throw new ArgumentNullException("", "El campo Body es requerido");
            }

            if (!To.Any())
            {
                throw new ArgumentNullException("", "El campo To al menos requiere un destinatario");
            }

            if (From is null)
            {
                From = new MailAddress(_configuration["Email:DefaultFrom"]);
            }
        }

        /// <summary>
        /// Registra el email en la base de datos.
        /// </summary>
        private async Task _registerLogEmail()
        {
            var addresses = new List<string>();
            foreach (var to in To)
            {
                addresses.Add(to.Address);
            }

            var registerLogEmail = new LogEmail
            {
                From = From.Address,
                To = string.Join(", ", addresses),
                Subject = Subject,
                Message = Body,
                SendAt = DateTime.Now
            };
            await _logEmailRepository.CreateAsync(registerLogEmail);
        }

        /// <summary>
        /// Log del email enviado.
        /// </summary>
        private void _loggerEmail()
        {
            var logEmail = new StringBuilder();
            logEmail.Append("\n=============================================\n");
            logEmail.Append($"Subject: {Subject}\n");
            logEmail.Append($"From: {From}\n");

            var toList = new List<string>();
            foreach (var to in To)
            {
                toList.Add(to.Address);
            }

            logEmail.Append(string.Join(", ", toList));
            logEmail.Append("\n=============================================\n");
            logEmail.Append($"{Body}\n");
            logEmail.Append("=============================================\n");
            _logger.LogInformation(logEmail.ToString());
        }
    }
}
