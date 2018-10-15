using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class AccountAdminRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "admin_account_list",
                template: "admin/account/list/",
                defaults: new { controller = "AccountAdmin", action = "Index" }
            );

            routes.MapRoute(
                name: "admin_account_edit",
                template: "admin/account/edit/{userId}/",
                defaults: new { controller = "AccountAdmin", action = "EditAccount" }
            );

            routes.MapRoute(
                name: "admin_account_create",
                template: "admin/account/create/",
                defaults: new { controller = "AccountAdmin", action = "Create" }
            );
        }
    }
}
