using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities;
using ApplicationCore.Data.Identity;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Services.EmailServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Extensions;
using Web.ViewModels.ContactViewModels;

namespace Web.Controllers.Contacts
{
    public class ContactController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IContactRepository _contactRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ContactController(
            UserManager<AppUser> userManager,
            IContactRepository contactRepository,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _contactRepository = contactRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                model.EmailFrom = user.Email;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newContact = new Contact
            {
                EmailFrom = model.EmailFrom,
                Subject = model.Subject,
                Message = model.Message
            };

            await _contactRepository.CreateAsync(newContact);
            var siteName = _configuration["Site:SiteName"];
            var admins = await _userManager.GetUsersInRoleAsync("Admins");
            _emailService.To = admins
                .Select(u => new MailAddress(u.Email))
                .ToList();
            _emailService.Subject = $"Nuevo mensaje de contacto {siteName}";
            await _emailService.SendEmailAsync("Contact", model);

            this.AddMessage("success", "Mensaje enviado con éxito");
            return Redirect("/");
        }
    }
}
