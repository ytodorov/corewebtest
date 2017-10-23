using DimoPdfToExcelWeb.BusinessLogic;
using DimoPdfToExcelWeb.Data;
using DimoPdfToExcelWeb.Models;
using DimoPdfToExcelWeb.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using System;
using WebMarkupMin.AspNetCore2;
using WebMarkupMin.MsAjax;

namespace DimoPdfToExcelWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();
            //app.UseStaticFiles();
            //https://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 240;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                    var file = ctx.File.PhysicalPath;
                    var file2 = ctx.File.Name;
                    // filter by js or css
                    ctx.Context.Response.Headers[HeaderNames.Vary] = "Accept-Encoding";
                }
            });

            app.UseAuthentication();

            app.UseWebMarkupMin();

            RewriteOptions ro = new RewriteOptions();

            var cd = Environment.MachineName;

            if (!cd.ToUpperInvariant().Contains("YORDAN".ToUpperInvariant()))
            {
                ro.AddRedirectToHttps();
            }

            ro.AddIISUrlRewrite(env.ContentRootFileProvider, "web.config");

            app.UseRewriter(ro);

            app.UseMvc(routes =>
           {
               routes.MapRoute(
                   name: "default",
                   template: "{controller=Home}/{action=Index}/{id?}");
           });

            // Configure Kendo UI
            app.UseKendo(env);

            Utils.PopulateHungarianMappingDictionaries(env.WebRootPath);
            Utils.PopulateSerbianMappingDictionaries(env.WebRootPath);
            Utils.PopulateCroatianMappingDictionaries(env.WebRootPath);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "327729092284-8umlocf6nrn9nn7viu8fm9b8dlmiu3jf.apps.googleusercontent.com";
                googleOptions.ClientSecret = "k7fbt8DFAgLKIRDZ7yei4yhx";
            });

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});

            services.AddMvc()
                   .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddWebMarkupMin(
        options =>
        {
            options.AllowMinificationInDevelopmentEnvironment = true;
            options.AllowCompressionInDevelopmentEnvironment = true;
        })
        .AddHtmlMinification(
            options =>
            {
                options.MinificationSettings.RemoveRedundantAttributes = true;
                options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
                options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;

                options.CssMinifierFactory = new MsAjaxCssMinifierFactory();
                options.JsMinifierFactory = new MsAjaxJsMinifierFactory();
            })
        .AddHttpCompression();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
            // Default
            "text/plain",
            "text/css",
            "application/javascript",
            "text/html",
            "application/xml",
            "text/xml",
            "application/json",
            "text/json",
            // Custom
            "image/svg+xml",
            "application/font-woff2"
                };
            });

            // Add Kendo UI services to the services container
            services.AddKendo();
        }
    }
}