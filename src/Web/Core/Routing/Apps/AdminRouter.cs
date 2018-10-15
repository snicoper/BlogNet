using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class AdminRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "admin_default",
                template: "admin/",
                defaults: new { controller = "Admin", action = "Index" }
            );
        }
    }
}
