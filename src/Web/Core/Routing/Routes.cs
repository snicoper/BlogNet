using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Web.Core.Routing.Apps;

namespace Web.Core.Routing
{
    public static class Routes
    {
        public static void Initialize(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                AccountAdminRouter.Initialize(routes);
                AccountRouter.Initialize(routes);
                AdminRouter.Initialize(routes);
                ArticleSubscribeRouter.Initialize(routes);
                ArticleRouter.Initialize(routes);
                ArticleAdminRouter.Initialize(routes);
                ContactRouter.Initialize(routes);
                ContactAdminRouter.Initialize(routes);
                ErrorsRouter.Initialize(routes);
                LogEmailRouter.Initialize(routes);
                LogErrorRouter.Initialize(routes);
                PagesRouter.Initialize(routes);
                TagAdminRouter.Initialize(routes);

                // Home page
                routes.MapRoute(
                    name: "home_page",
                    template: "",
                    defaults: new { controller = "Article", action = "List" }
                );
            });
        }
    }
}
