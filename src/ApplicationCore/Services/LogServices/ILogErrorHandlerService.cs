using System;

namespace ApplicationCore.Services.LogServices
{
    public interface ILogErrorHandlerService
    {
        void Attach(ILogErrorService logErrorService);
        void Detach(ILogErrorService logErrorService);
        void Notify(Exception exception, string username, string urlPath);
    }
}
