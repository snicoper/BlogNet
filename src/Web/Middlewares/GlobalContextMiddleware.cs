using System.Threading.Tasks;
using ApplicationCore.Core;
using ApplicationCore.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ApplicationCore.Middlewares
{
    public class GlobalContextMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context,
            IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            GlobalContextCore.SiteName = configuration["Site:SiteName"];
            GlobalContextCore.Scheme = configuration["Site:Scheme"];
            GlobalContextCore.SiteUrl = configuration["Site:SiteUrl"];

            await _next(context);
        }
    }
}
