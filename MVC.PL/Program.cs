using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVC.DAL.Data;
using MVC.DAL.Models;
using MVC.PL.Extentisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.PL
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

			#region Configure Services
			Builder.Services.AddControllersWithViews(); //allow services for mvc project
														//allow depenncy injection for mvcdbcontext and optiondbcontext
			Builder.Services.AddDbContext<MvcDbContext>(options => options.UseSqlServer(Builder.Configuration.GetConnectionString("defaultConnection")));

			Builder.Services.ApplyAppServices();

			Builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<MvcDbContext>()
				.AddDefaultTokenProviders();



			Builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Acount/SignIn";
				options.AccessDeniedPath = "/Home/Error";
				options.ExpireTimeSpan = TimeSpan.FromDays(5);

			});
			//Create New Token Schema.
			Builder.Services.AddAuthentication().AddCookie("Hamo", options =>
			{
				options.LoginPath = "/Acount/SignIn";
				options.AccessDeniedPath = "/Home/Error";
				options.ExpireTimeSpan = TimeSpan.FromDays(1);

			});
			#endregion

			var app= Builder.Build();

			#region Congigure MiddleWares
			if (app.Environment.IsDevelopment())
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
			#endregion


			app.Run();

		}

      
    }
}
