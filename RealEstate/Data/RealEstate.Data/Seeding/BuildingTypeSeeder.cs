namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class BuildingTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.BuildingTypes.Any())
            {
                return;
            }

            var buildingTypes = new List<BuildingType>
            {
                 new BuildingType
                {
                    Name = "Тухла",
                },
                 new BuildingType
                {
                    Name = "Панел",
                },
                 new BuildingType
                {
                    Name = "ЕПК",
                },
                 new BuildingType
                 {
                     Name = "ПК",
                 },
                 new BuildingType
                 {
                     Name = "Гредоред",
                 },
            };

            await dbContext.BuildingTypes.AddRangeAsync(buildingTypes);
        }
    }
}
