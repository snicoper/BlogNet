using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Interfaces;

namespace ApplicationCore.Data.Repositories
{
    public class LogEmailRepository : BaseRepository<LogEmail>, ILogEmailRepository
    {
        public LogEmailRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
