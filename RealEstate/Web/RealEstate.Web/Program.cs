// Add migrations related user to property

namespace RealEstate.Web
{
    using System;
    using System.Reflection;

    using Hangfire;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using RealEstate.Data;
    using RealEstate.Data.Common;
    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Data.Repositories;
    using RealEstate.Data.Seeding;
    using RealEstate.Services.Data;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;
    using RealEstate.Services.LocationScraperService;
    using RealEstate.Services.Mapping;
    using RealEstate.Services.Messaging;
    using RealEstate.Web.ViewModels;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlServer(sqlConnectionString));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "AutenticationCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Home/Index";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews(
                options =>
                {
                    // options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(configuration);

            services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings(x => x.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
               .UseSqlServerStorage(sqlConnectionString));

            services.AddHangfireServer();

            ConfigureRepositorues(services);
            ConfigureApplicationServices(services);

        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder()
                    .SeedAsync(dbContext, serviceScope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseHangfireDashboard();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHangfireDashboard();

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
        }

        private static void ConfigureRepositorues(IServiceCollection services)
        {
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();
        }

        private static void ConfigureApplicationServices(IServiceCollection services)
        {
            services.AddScoped<IEmailSender, NullMessageSender>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRegionScraperService, LocationScraperService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<IBuildingTypeService, BuildingTypeService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IPopulatedPlaceService, PopulatedPlaceService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IConditionService, ConditionService>();
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IHeatingService, HeatingService>();
            services.AddScoped<IDetailService, DetailService>();
        }
    }
}
