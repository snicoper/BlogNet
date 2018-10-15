using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels.ContactAdminViewModels;

namespace Web.Api
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admins")]
    public class ContactController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public ContactController(
            IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [HttpGet]
        public IQueryable<Contact> GetAll()
        {
            return _contactRepository.GetAll();
        }

        [HttpGet("paginate")]
        public ListViewModel GetAll(ListViewModel paginate)
        {
            var query = GetAll();
            if (!string.IsNullOrEmpty(paginate.EmailFrom))
            {
                query = query.Where(c => c.EmailFrom.ToLower().Contains(paginate.EmailFrom.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.Subject))
            {
                query = query.Where(c => c.Subject.ToLower().Contains(paginate.Subject.ToLower()));
            }

            if (!string.IsNullOrEmpty(paginate.Message))
            {
                query = query.Where(c => c.Message.ToLower().Contains(paginate.Message.ToLower()));
            }

            if (DateTime.TryParse(paginate.SendAt, out var sendAt))
            {
                query = query.Where(c => c.SendAt.ToShortDateString() == sendAt.ToShortDateString());
            }

            switch (paginate.HasRead)
            {
                case HasReadStatus.Read:
                    query = query.Where(c => c.HasRead);
                    break;
                case HasReadStatus.Unread:
                    query = query.Where(c => c.HasRead == false);
                    break;
                case HasReadStatus.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            paginate.Paginate(query);
            return paginate;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var contact = _contactRepository.GetById(id);
            if (contact is null)
            {
                return NotFound();
            }

            return new ObjectResult(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var contact = _contactRepository.GetById(id);
            if (contact is null)
            {
                return NotFound();
            }

            await _contactRepository.RemoveAsync(contact);
            return new NoContentResult();
        }
    }
}
