using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class ArticleAdminRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "article_admin_list",
                template: "admin/blog/article/list/",
                defaults: new { controller = "ArticleAdmin", action = "List" }
            );

            routes.MapRoute(
                name: "article_admin_create",
                template: "admin/blog/article/create/",
                defaults: new { controller = "ArticleAdmin", action = "Create" }
            );

            routes.MapRoute(
                name: "article_admin_edit",
                template: "admin/blog/article/edit/{id}/",
                defaults: new { controller = "ArticleAdmin", action = "Edit" }
            );

            routes.MapRoute(
                name: "article_admin_delete",
                template: "admin/blog/article/delete/{id}/",
                defaults: new { controller = "ArticleAdmin", action = "Delete" }
            );
        }
    }
}
