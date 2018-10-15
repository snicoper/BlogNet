using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class LogErrorRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "log_error_index",
                template: "admin/log-error/",
                defaults: new { controller = "LogError", action = "Index" }
            );

            routes.MapRoute(
                name: "log_error_delete_all",
                template: "admin/log-error/delete/all/",
                defaults: new { controller = "LogError", action = "DeleteAll" }
            );

            routes.MapRoute(
                name: "log_error_mark_all_read",
                template: "admin/log-error/read/all/",
                defaults: new { controller = "LogError", action = "MarkAllRead" }
            );
        }
    }
}
