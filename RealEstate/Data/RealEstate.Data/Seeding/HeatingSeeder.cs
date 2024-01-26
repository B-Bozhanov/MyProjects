namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class HeatingSeeder : DataSeeder, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Heating.Any())
            {
                return;
            }

            var heatings = this.GetDataFromJson<Heating>(nameof(Heating), serviceProvider);
            await AddDataAsync(dbContext, heatings);
        }

        private static async Task AddDataAsync(ApplicationDbContext dbContext, IEnumerable<Heating> heatings)
        {
            foreach (var heating in heatings)
            {
                await dbContext.Heating.AddAsync(heating);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
