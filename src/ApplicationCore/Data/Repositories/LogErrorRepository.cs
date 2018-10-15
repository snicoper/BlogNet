using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Interfaces;

namespace ApplicationCore.Data.Repositories
{
    public class LogErrorRepository : BaseRepository<LogError>, ILogErrorRepository
    {
        public LogErrorRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<LogError> GetUnReadLogErrors()
        {
            return GetAll().Where(l => l.Checked == false);
        }

        public void MarkAllChecked()
        {
            var logErrors = GetUnReadLogErrors();

            foreach (var logError in logErrors)
            {
                logError.Checked = true;
                DbContext
                    .Entry(logError)
                    .Property(nameof(LogError.Checked))
                    .IsModified = true;
            }

            DbContext.SaveChanges();
        }
    }
}
