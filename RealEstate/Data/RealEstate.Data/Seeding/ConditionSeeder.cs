namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class ConditionSeeder : DataSeeder, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Condition.Any())
            {
                return;
            }

            var conditions = this.GetDataFromJson<Condition>(nameof(Condition), serviceProvider);
            await AddDataAsync(dbContext, conditions);
        }

        private static async Task AddDataAsync(ApplicationDbContext dbContext, IEnumerable<Condition> conditions)
        {
            foreach (var condition in conditions)
            {
                await dbContext.Condition.AddAsync(condition);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
