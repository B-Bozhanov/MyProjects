using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data;
// Add migrations related user to property

namespace RealEstate.Web
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Hangfire;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;

    using RealEstate.Common;
    using RealEstate.Data;
    using RealEstate.Data.Common;
    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Data.Repositories;
    using RealEstate.Data.Seeding;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Services.Messaging;
    using RealEstate.Web.Hubs;
    using RealEstate.Web.Infrastructure.DatabaseModels;
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
            string sqlConnectionString = GetConnectionString(configuration, "SqlConnectionString");

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
                options.LoginPath = "/Login";
                options.LogoutPath = "/Home/Index";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            });

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .AddRazorRuntimeCompilation()
                .AddRazorOptions(opt =>
                {
                    opt.ViewLocationFormats.Add("/{0}.cshtml");
                });
            services.AddAntiforgery(x => x.HeaderName = GlobalConstants.AjaxAntiforgeryTokenName);

            services.AddRazorPages();
            services.AddSignalR();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(configuration);

            services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings(x => x.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
               .UseSqlServerStorage(sqlConnectionString));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHangfireServer();

            //TODO: Remove AddServices with reflection:
            services.AddServices(new Type[] { typeof(IPropertyService), typeof(IHangfireWrapperService)});

            ConfigureRepositorues(services);
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            // ConfigureApplicationServices(services);
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
            app.MapHub<PropertyHub>("/propertyHub");
        }

        private static void ConfigureRepositorues(IServiceCollection services)
        {
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();
        }

        private static string GetConnectionString(IConfiguration configuration, string databaseConnectionStringName)
        {
            var databaseConfigurationSection = configuration
                .GetSection("ConnectionStrings")
                .GetChildren()
                .FirstOrDefault(x => x.Key == databaseConnectionStringName)
                ?? throw new ArgumentNullException("The Database connection string can not be found");

            var databaseModel = databaseConfigurationSection.Get<SqlDatabaseModel>();

            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                ConnectionString = databaseModel.ConnectionString,
                Password = databaseModel.Password,
                UserID = databaseModel.UserId,
            };

            return sqlConnectionStringBuilder.ConnectionString;
        }

        //private static void ConfigureApplicationServices(IServiceCollection services)
        //{
        //    services.AddScoped<IEmailSender, NullMessageSender>();
        //    services.AddScoped<ISettingsService, SettingsService>();
        //    services.AddScoped<IPropertyService, PropertyService>();
        //    services.AddScoped<IImageService, ImageService>();
        //    services.AddScoped<IRegionScraperService, LocationScraperService>();
        //    services.AddScoped<ILocationService, LocationService>();
        //    services.AddScoped<IPropertyTypeService, PropertyTypeService>();
        //    services.AddScoped<IBuildingTypeService, BuildingTypeService>();
        //    services.AddScoped<ILocationService, LocationService>();
        //    services.AddScoped<IPopulatedPlaceService, PopulatedPlaceService>();
        //    services.AddScoped<IAccountService, AccountService>();
        //    services.AddScoped<IConditionService, ConditionService>();
        //    services.AddScoped<IEquipmentService, EquipmentService>();
        //    services.AddScoped<IHeatingService, HeatingService>();
        //    services.AddScoped<IDetailService, DetailService>();
        //    services.AddScoped<IHangfireWrapper, HangfireWrapper>();
        //}
    }
}
