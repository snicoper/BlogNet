using System.Collections.Generic;
using ApplicationCore.Data.Entities;

namespace ApplicationCore.Data.Interfaces
{
    public interface ILogErrorRepository : IRepository<LogError>
    {
        IEnumerable<LogError> GetUnReadLogErrors();
        void MarkAllChecked();
    }
}
