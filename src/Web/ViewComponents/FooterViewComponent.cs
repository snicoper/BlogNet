using System.Threading.Tasks;
using ApplicationCore.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.ViewModels;

namespace Web.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public FooterViewComponent(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var email = string.Empty;
            var model = new FooterViewModel
            {
                SiteName = _configuration["Site:SiteName"]
            };

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user != null)
                {
                    // Si el usuario esta subscrito, no mostrara el input.
                    if (user.SubscribeArticleId > 0)
                    {
                        model.IsSubscribed = true;
                    }
                    else
                    {
                        email = user.Email;
                    }
                }
            }

            model.EmailSubscribe = email;
            return View(model);
        }
    }
}
