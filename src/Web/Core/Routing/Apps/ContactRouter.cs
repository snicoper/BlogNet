using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class ContactRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "contact_create",
                template: "contact/",
                defaults: new { controller = "Contact", action = "Create" }
            );
        }
    }
}
