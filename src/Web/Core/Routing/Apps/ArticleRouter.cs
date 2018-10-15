using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class ArticleRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "blog_default",
                template: "blog/",
                defaults: new { controller = "Article", action = "List" }
            );

            routes.MapRoute(
                name: "article_list_with_page",
                template: "blog/list/page{page}/",
                defaults: new { controller = "Article", action = "List" }
            );

            routes.MapRoute(
                name: "article_list_by_tag",
                template: "blog/tag/{tagSlug:regex([a-z0-9\\-]+)}/list/page{page}/",
                defaults: new { controller = "Article", action = "ListByTagName" }
            );

            routes.MapRoute(
                name: "article_details",
                template: "blog/article/{slug:regex([a-z0-9\\-]+)}/",
                defaults: new { controller = "Article", action = "Details" }
            );

            routes.MapRoute(
                name: "article_search",
                template: "blog/search/page{page}/",
                defaults: new { controller = "Article", action = "Search" }
            );

            routes.MapRoute(
                name: "blog_rss",
                template: "blog/rss/",
                defaults: new { controller = "Article", action = "Rss" }
            );

            routes.MapRoute(
                name: "article_recommend_by_slug",
                template: "blog/recommend/{slug:regex([a-z0-9\\-]+)}/",
                defaults: new { controller = "Article", action = "Recommend" }
            );
        }
    }
}
