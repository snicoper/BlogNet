using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Web.ViewComponents
{
    public class GAnalyticViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;

        public GAnalyticViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IViewComponentResult Invoke()
        {
            return View((object)_configuration["GAnalyticTag"]);
        }
    }
}
