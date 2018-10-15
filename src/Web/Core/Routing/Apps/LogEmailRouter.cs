using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class LogEmailRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "log_email_index",
                template: "admin/log-email/",
                defaults: new { controller = "LogEmail", action = "Index" }
            );

            routes.MapRoute(
                name: "log_email_details",
                template: "admin/log-email/details/{id}/",
                defaults: new { controller = "LogEmail", action = "Details" }
            );

            routes.MapRoute(
                name: "log_email_delete",
                template: "admin/log-email/delete/{id}/",
                defaults: new { controller = "LogEmail", action = "Delete" }
            );

            routes.MapRoute(
                name: "log_email_delete_all",
                template: "admin/log-email/delete/all/",
                defaults: new { controller = "LogEmail", action = "DeleteAll" }
            );
        }
    }
}
