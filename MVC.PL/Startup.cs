using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.BLL.Repositories;
using MVC.DAL.Data;
using MVC.DAL.Models;
using MVC.PL.Extentisions;
using MVC.PL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.PL
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
            services.AddControllersWithViews(); //allow services for mvc project
            //allow depenncy injection for mvcdbcontext and optiondbcontext
            services.AddDbContext<MvcDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("defaultConnection")));

            services.ApplyAppServices();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MvcDbContext>()
				.AddDefaultTokenProviders();
			


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Acount/SignIn";
                options.AccessDeniedPath = "/Home/Error";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);

			});

            services.AddAuthentication(options =>
            {

               

            }).AddCookie("Hamo", options =>
            {
                options.LoginPath = "/Acount/SignIn";
                options.AccessDeniedPath = "/Home/Error";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);

            });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
