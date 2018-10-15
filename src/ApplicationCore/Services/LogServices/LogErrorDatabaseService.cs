using System;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Interfaces;

namespace ApplicationCore.Services.LogServices
{
    public class LogErrorDatabaseService : ILogErrorService
    {
        private readonly ILogErrorRepository _logErrorRepository;

        public LogErrorDatabaseService(ILogErrorRepository logErrorRepository)
        {
            _logErrorRepository = logErrorRepository;
        }

        public void Notify(Exception exception, string username, string urlPath)
        {
            var logError = new LogError
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Username = username,
                Path = urlPath,
                CreateAt = DateTime.Now
            };
            _logErrorRepository.Create(logError);
        }
    }
}
