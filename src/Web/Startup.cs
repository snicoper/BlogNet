using ApplicationCore.Data;
using ApplicationCore.Data.Identity;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Services.EmailServices;
using ApplicationCore.Services.ImageServices;
using ApplicationCore.Services.LogServices;
using ApplicationCore.Services.ViewServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ApplicationCore.Data.Repositories;
using ApplicationCore.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Web.Core.Routing;
using Web.Filters;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ILogErrorHandlerService, LogErrorHandlerService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IUploadImageService, UploadImageService>();

            // Repositories
            services.AddTransient<ILogErrorRepository, LogErrorRepository>();
            services.AddTransient<ILogEmailRepository, LogEmailRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IArticleRepository, ArticleRepository>();
            services.AddTransient<ISubscribeArticleRepository, SubscribeArticleRepository>();

            // PostgreSQL
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("ApplicationDbConnection"), builder =>
                {
                    builder.MigrationsAssembly("ApplicationCore");
                })
            );

            // Identity
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login/";
                options.LogoutPath = "/account/logout/";
            });

            // Routing
            services.AddRouting(options =>
            {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });

            // Localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("es-ES"),
                    new CultureInfo("es")
                };

                options.DefaultRequestCulture = new RequestCulture("es-ES");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddSession();

            // Razor
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("~/Views/Shared/Emails/{0}.cshtml");
            });

            // MVC
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Culture = CultureInfo.CurrentCulture;
                options.SerializerSettings.DateFormatString = "dd/MM/yyyy HH:mm";
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }).AddMvcOptions(options =>
            {
                options.Filters.Add(typeof(ExceptionHandlingFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Media path
            var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseExceptionHandler("/error/");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRequestLocalization();
            app.UseMiddleware<GlobalContextMiddleware>();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesPath),
                RequestPath = "/media"
            });

            // Custom pages errors
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    context.Request.Path = "/error/404/";
                    await next();
                }
            });

            Routes.Initialize(app);
        }
    }
}
