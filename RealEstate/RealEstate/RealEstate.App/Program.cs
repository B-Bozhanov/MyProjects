namespace RealEstate.App
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data;
    using RealEstate.Data.Extencions;
    using RealEstate.Data.Interfaces;
    using RealEstate.Data.Repositories;
    using RealEstate.Services;
    using RealEstate.Services.Interfaces;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureDataServices(builder.Services, builder.Configuration);
            ConfigureApplicationServices(builder.Services);
            var app = builder.Build();
            ConfigureApp(app);
            app.Run();
        }

        private static void ConfigureApp(WebApplication app)
        {
            app.MigrateDatabase();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
        }

        private static void ConfigureApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IImportService, ImportService>();
            services.AddTransient<IPropertyService, PropertyService>();
        }

        private static void ConfigureDataServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => 
                 options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();

            // Configure repositories:

            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
            services.AddScoped<IBuildingTypeRepository, BuildingTypeRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IUserContactsRepository, UserContactsRepository>();
        }
    }
}