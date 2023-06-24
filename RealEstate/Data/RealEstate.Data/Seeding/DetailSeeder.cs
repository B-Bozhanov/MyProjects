namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class DetailSeeder : DataSeeder, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Details.Any())
            {
                return;
            }

            var detaills =  this.GetDataFromJson<Detail>(nameof(Detail));
            await AddDataAsync(dbContext, detaills);
        }

        private static async Task AddDataAsync(ApplicationDbContext dbContext, IEnumerable<Detail> details)
        {
            foreach (var detail in details)
            {
                await dbContext.Details.AddAsync(detail);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
