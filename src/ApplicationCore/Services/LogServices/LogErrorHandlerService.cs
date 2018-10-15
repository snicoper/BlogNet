using System;
using System.Collections.Generic;

namespace ApplicationCore.Services.LogServices
{
    public class LogErrorHandlerService : ILogErrorHandlerService
    {
        private readonly List<ILogErrorService> _logErrorServices;

        public LogErrorHandlerService()
        {
            _logErrorServices = new List<ILogErrorService>();
        }

        public void Attach(ILogErrorService logErrorService)
        {
            _logErrorServices.Add(logErrorService);
        }

        public void Detach(ILogErrorService logErrorService)
        {
            _logErrorServices.Remove(logErrorService);
        }

        public void Notify(Exception exception, string username, string urlPath)
        {
            foreach (var errorService in _logErrorServices)
            {
                errorService.Notify(exception, username, urlPath);
            }
        }
    }
}
