using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ApplicationCore.Services.EmailServices
{
    public interface IEmailService
    {
        bool IsHtml { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        List<MailAddress> To { get; set; }
        MailAddress From { get; set; }
        bool RegisterEmail { get; set; }

        Task SendEmailAsync(string viewName, object model);
        Task SendEmailAsync();
    }
}
