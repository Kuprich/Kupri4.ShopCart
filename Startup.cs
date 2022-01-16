using Kupri4.ShopCart.Infrastructure;
using Kupri4.ShopCart.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Kupri4.ShopCart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShopCartDbContext>(options =>
                options.UseSqlite($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "ShopCart.Db")}"));

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ShopCartDbContext>()
                .AddDefaultTokenProviders();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddMemoryCache();
            services.AddAuthentication();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7);
            });
            services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Pages}/{p:int?}",
                  defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Pages}/{action=Index}/{id?}");



                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "{controller}/{p:int?}",
                    defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "{controller}/{categorySlug}/{p:int?}",
                    defaults: new { action = "ProductsByCategory" });

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "{controller}",
                    defaults: new { action = "Index" });



                endpoints.MapControllerRoute(
                    name: "pages",
                    pattern: "{slug?}",
                    defaults: new { controller = "Pages", action = "Page" });


                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");

            });

        }
    }
}
