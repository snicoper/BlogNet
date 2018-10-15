using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class ErrorsRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "error_index",
                template: "error/",
                defaults: new { controller = "Error", action = "Index" }
            );

            routes.MapRoute(
                name: "error_404",
                template: "error/404/",
                defaults: new { controller = "Error", action = "Error404" }
            );
        }
    }
}
