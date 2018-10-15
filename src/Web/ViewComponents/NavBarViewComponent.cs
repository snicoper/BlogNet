using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Web.ViewModels;

namespace Web.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        private readonly IContactRepository _contactRepository;

        public NavBarViewComponent(
            IConfiguration configuration,
            IContactRepository contactRepository)
        {
            _configuration = configuration;
            _contactRepository = contactRepository;
        }

        public IViewComponentResult Invoke()
        {
            var model = new NavBarViewModel
            {
                SiteName = _configuration["Site:SiteName"]
            };

            // Numero de mensajes de contacto.
            if (User.Identity.IsAuthenticated && User.IsInRole("Admins"))
            {
                model.ContactUnreadMessages = _contactRepository.GetUnreadMessages().Count();
            }

            return View(model);
        }
    }
}
