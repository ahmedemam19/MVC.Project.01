using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVC.Project.DAL.Data;
using MVC.Project.DAL.Models;
using MVC.Project.PL.Extensions;
using MVC.Project.PL.Helpers;
using System;

namespace MVC.Project.PL
{
	public class Program
    {
        public static void Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);


			#region Configure Services

			webApplicationBuilder.Services.AddControllersWithViews(); // Register Built-in Services Required By MVC

			webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseLazyLoadingProxies().UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
			});


			//ApplicationServicesExtension.AddApplicationServices(services); // Static Method , Found in Folder [ Extensions ]
			webApplicationBuilder.Services.AddApplicationServices(); // Extension Method


			webApplicationBuilder.Services.AddAutoMapper(mapper => mapper.AddProfile(new MappingProfile()));


			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequiredUniqueChars = 2;
				options.Password.RequireDigit = true;
				options.Password.RequireNonAlphanumeric = true; // @$%
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 5;

				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

			})
			  .AddEntityFrameworkStores<ApplicationDbContext>()
			  .AddDefaultTokenProviders();


			webApplicationBuilder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/SignIn";
				options.ExpireTimeSpan = TimeSpan.FromDays(1);
				options.AccessDeniedPath = "/Home/Error";

			});


			// By this we add new Schema named [ Hamada ]
			webApplicationBuilder.Services.AddAuthentication(options =>
			{
				//options.DefaultAuthenticateScheme = "Identity.Application"; // Default Schema
				//options.DefaultChallengeScheme = "Identity.Application";

			})
				.AddCookie("Hamada", options =>
				{
					options.LoginPath = "/Account/SignIn";
					options.ExpireTimeSpan = TimeSpan.FromDays(1);
					options.AccessDeniedPath = "/Home/Error";
				});

			#endregion



			#region Configure Kestrel MiddleWare


			var app = webApplicationBuilder.Build();


			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				//app.UseMigrationsEndPoint();
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


			app.Run(); // Application is ready for Requests


		}

        
    }
}
