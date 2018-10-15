using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class TagAdminRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "admin_tag_list",
                template: "admin/blog/tag/list/",
                defaults: new { controller = "TagAdmin", action = "List" }
            );

            routes.MapRoute(
                name: "admin_tag_create",
                template: "admin/blog/tag/create/",
                defaults: new { controller = "TagAdmin", action = "Create" }
            );

            routes.MapRoute(
                name: "admin_tag_edit",
                template: "admin/blog/tag/edit/{id}/",
                defaults: new { controller = "TagAdmin", action = "Edit" }
            );

            routes.MapRoute(
                name: "admin_tag_delete",
                template: "admin/blog/tag/delete/{id}/",
                defaults: new { controller = "TagAdmin", action = "Delete" }
            );
        }
    }
}
