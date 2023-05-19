namespace RealEstate.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using RealEstate.Common;
    using RealEstate.Data.Models;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var administratorsNames = GlobalConstants.GetAdministrators();

            foreach (var administratorUserName in administratorsNames)
            {
                if (!dbContext.Users.Any(u => u.UserName == administratorUserName.UserName))
                {
                    ApplicationUser user = new()
                    {
                        UserName = administratorUserName.UserName,
                        Email = administratorUserName.Email,
                    };

                    await userManager.CreateAsync(user, administratorUserName.Password);

                    await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
            }
        }
    }
}
