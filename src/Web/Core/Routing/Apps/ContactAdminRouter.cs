using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class ContactAdminRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "admin_contact_list",
                template: "admin/contact/",
                defaults: new { controller = "ContactAdmin", action = "List" }
            );

            routes.MapRoute(
                name: "admin_contact_details",
                template: "admin/contact/details/{id?}/",
                defaults: new { controller = "ContactAdmin", action = "Details" }
            );

            routes.MapRoute(
                name: "admin_contact_delete",
                template: "admin/contact/delete/{id?}/",
                defaults: new { controller = "ContactAdmin", action = "Delete" }
            );
        }
    }
}
