using System.Linq;
using ApplicationCore.Data.Entities;

namespace ApplicationCore.Data.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        IQueryable<Contact> GetUnreadMessages();
    }
}
