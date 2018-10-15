using System.Threading.Tasks;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Extensions;
using Web.ViewModels.ContactAdminViewModels;

namespace Web.Controllers.Contacts
{
    [Authorize(Roles = "Admins")]
    public class ContactAdminController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public ContactAdminController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IActionResult List()
        {
            var model = new ListViewModel();
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var contact = _contactRepository.GetById(id);
            if (contact is null)
            {
                return NotFound();
            }
            contact.HasRead = true;
            await _contactRepository.UpdateAsync(contact);
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = _contactRepository.GetById(id);
            if (contact is null)
            {
                return NotFound();
            }
            await _contactRepository.RemoveAsync(contact);
            this.AddMessage("success", "Mensaje eliminado con éxito");
            return RedirectToAction(nameof(List));
        }
    }
}
