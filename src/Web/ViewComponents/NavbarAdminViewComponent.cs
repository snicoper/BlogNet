using System.Linq;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.ViewComponents
{
    public class NavbarAdminViewComponent : ViewComponent
    {
        private readonly IContactRepository _contactRepository;
        private readonly ILogErrorRepository _logErrorRepository;

        public NavbarAdminViewComponent(
            IContactRepository contactRepository,
            ILogErrorRepository logErrorRepository)
        {
            _contactRepository = contactRepository;
            _logErrorRepository = logErrorRepository;
        }

        public IViewComponentResult Invoke()
        {
            var unreadMessages = _contactRepository.GetUnreadMessages();
            var model = new NavbarAdminViewModel
            {
                UnreadMessagesContact = unreadMessages.Count(),
                UnreadLogError = _logErrorRepository.GetUnReadLogErrors().Count()
            };
            return View(model);
        }
    }
}
