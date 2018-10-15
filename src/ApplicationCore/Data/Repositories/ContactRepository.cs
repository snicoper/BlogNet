using System.Linq;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Interfaces;

namespace ApplicationCore.Data.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<Contact> GetUnreadMessages()
        {
            return GetAll().Where(c => c.HasRead == false);
        }
    }
}
