namespace RealEstate.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using RealEstate.Common;
    using RealEstate.Data.Models;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var administrators = configuration.GetSection("Administrators").GetChildren();
            foreach (var administrator in administrators)
            {
                var user = administrator.Get<ApplicationUser>();
                var userPassword = administrator.GetValue<string>("Password");

                await userManager.CreateAsync(user, userPassword);
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
