using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Important Note : DbContext Is Scoped by Default in Dependency Injection.

            //// One Object per Lifetime Application and Disposed after Application Shutdown
            //builder.Services.AddSingleton<>();
            //// One Object per Client Request and Disposed after Request
            //builder.Services.AddScoped<>();
            //// New Object per Injection and Disposed after Use
            //builder.Services.AddTransient<>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Add Scope (Object of UnitOfWork) per request and Dispose this Scope when request is ended

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
