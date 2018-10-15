using System;

namespace ApplicationCore.Services.LogServices
{
    public interface ILogErrorService
    {
        void Notify(Exception exception, string username, string urlPath);
    }
}
