using GymManagementBLL;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            builder.Services.AddScoped<ISessionRepository, SessionRepository>(); // Dependency injection container for SessionRepository.
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>(); // Dependency injection container for AnalyticsService.
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            // Registers MappingProfile in DI so AutoMapper builds its configuration.
            // When IMapper is injected, DI provides a ready Mapper that applies all rules from the Profile.
            builder.Services.AddAutoMapper(x => x.AddProfile(new MappingProfile())); // Register AutoMapper and add MappingProfile

            var app = builder.Build();

            // Run seeding after app.Build() because services like DbContext are only available after the app is built.
            #region Data seeding

            // Using 'await' here makes sure the database seeding process finishes before the app starts handling requests.
            // Without 'await', the app might start running while the database is still being seeded, causing missing or incomplete data.
            // That's why the Main method needs to be async — to allow waiting (asynchronously) for long-running startup tasks like seeding.
            // ========================================================================

            using var scope = app.Services.CreateScope();

            var gymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>(); // get object from GymDbContext
            await GymDataSeeding.SeedData(gymDbContext); // send this object as a parameter to SeedData function.

            #endregion

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
            app.MapControllerRoute( // Define the default routing pattern
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}") // If no controller is specified in the URL, use HomeController and Index action by default
                .WithStaticAssets(); // Enable serving static files (int wwwroot) for this route

            app.Run();
        }
    }
}
