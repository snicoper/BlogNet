using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Web.Core.Routing.Apps
{
    public class ArticleSubscribeRouter
    {
        public static void Initialize(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "article_subscribe",
                template: "blog/subscribe/",
                defaults: new { controller = "ArticleSubscribe", action = "Subscribe" }
            );

            routes.MapRoute(
                name: "article_unsubscribe",
                template: "blog/unsubscribe/{token}/",
                defaults: new { controller = "ArticleSubscribe", action = "UnSubscribe" }
            );
        }
    }
}
