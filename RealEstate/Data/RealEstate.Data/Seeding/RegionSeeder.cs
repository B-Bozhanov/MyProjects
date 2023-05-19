namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    public class RegionSeeder : DataSeeder, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Regions.Any())
            {
                return;
            }

            var regions = this.GetData<Region>(nameof(Region));

            await AddDataAsync(dbContext, regions);
        }

        private static async Task AddDataAsync(ApplicationDbContext dbContext, IEnumerable<Region> regions)
        {
            var id = 0;

            foreach (var region in regions)
            {
                id++;

                var dbRegion = new Region
                {
                    Name = region.Name,
                };

                if (region.DownTown == null)
                {
                    continue;
                }

                var dbDownTown = new DownTown
                {
                    Name = region.DownTown.Name,
                };

                dbRegion.DownTown = dbDownTown;
                dbDownTown.Region = dbRegion;
                dbDownTown.RegionId = dbRegion.Id;

                foreach (var district in region.DownTown.Districts)
                {
                    var dbDistrict = new District
                    {
                        Name = district.Name,
                    };

                    dbDistrict.DownTown = dbDownTown;

                    await dbContext.Districts.AddAsync(dbDistrict);
                }

                foreach (var place in region.Places)
                {
                    var dbPlace = new Place
                    {
                        Name = place.Name,
                    };

                    dbPlace.Region = dbRegion;

                    await dbContext.Places.AddAsync(dbPlace);
                }
            }
        }
    }
}
