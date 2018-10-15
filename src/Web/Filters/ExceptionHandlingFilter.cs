using ApplicationCore.Data.Interfaces;
using ApplicationCore.Services.EmailServices;
using ApplicationCore.Services.LogServices;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Web.Filters
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IEmailService _emailService;
        private readonly ILogErrorHandlerService _logErrorHandlerService;

        public ExceptionHandlingFilter(
            IConfiguration configuration,
            ILogErrorRepository logErrorRepository,
            IEmailService emailService,
            ILogErrorHandlerService logErrorHandlerService)
        {
            _configuration = configuration;
            _logErrorRepository = logErrorRepository;
            _emailService = emailService;
            _logErrorHandlerService = logErrorHandlerService;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var urlString = context.HttpContext.Request.Path;
            urlString = urlString.HasValue ? urlString.Value : string.Empty;

            var username = context.HttpContext.User.Identity.Name ?? "Anónimo";
            _logErrorHandlerService.Attach(new LogErrorFileService());

            if (bool.Parse(_configuration["Email:NotifyExceptions"]) is true)
            {
                _logErrorHandlerService.Attach(new LogErrorEmailService(_configuration, _emailService));
            }

            _logErrorHandlerService.Attach(new LogErrorDatabaseService(_logErrorRepository));
            _logErrorHandlerService.Notify(exception, username, urlString);
        }
    }
}
