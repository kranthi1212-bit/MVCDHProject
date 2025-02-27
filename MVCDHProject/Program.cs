using MVCDHProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MVCDHProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            // Add services to the container.
            builder.Services.AddControllersWithViews(configure=>
            {
                var policy= new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                configure.Filters.Add(new AuthorizeFilter(policy));
            });
            builder.Services.AddScoped<ICustomerDAL,CustomerSqlDAL>();
            builder.Services.AddDbContext<MVCCoreDBContext>(options =>
                         options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(option => {
                option.Password.RequiredLength = 8;
                option.Password.RequireDigit = false;
                }).AddEntityFrameworkStores<MVCCoreDBContext>().AddDefaultTokenProviders();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.UseStatusCodePages();
                app.UseStatusCodePagesWithRedirects("/ClientError/{0}");
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler("/ServerError");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
