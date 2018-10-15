using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class PagesRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "page_sitemap",
                template: "sitemap.xml",
                defaults: new { controller = "Page", action = "Sitemap" }
            );

            routes.MapRoute(
                name: "page_about",
                template: "about/",
                defaults: new { controller = "Page", action = "About" }
            );

            routes.MapRoute(
                name: "page_cookie_consent",
                template: "cookie-consent/",
                defaults: new { controller = "Page", action = "CookieConsent" }
            );

            routes.MapRoute(
                name: "page_robots",
                template: "robots.txt",
                defaults: new { controller = "Page", action = "Robots" }
            );
        }
    }
}
