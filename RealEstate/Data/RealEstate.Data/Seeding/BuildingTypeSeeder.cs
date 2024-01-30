namespace RealEstate.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class BuildingTypeSeeder : DataSeederBase, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.BuildingTypes.Any())
            {
                return;
            }

            var buildingTypes = this.GetDataFromJson<BuildingType>(nameof(BuildingType), serviceProvider);

            await dbContext.BuildingTypes.AddRangeAsync(buildingTypes);
        }
    }
}
