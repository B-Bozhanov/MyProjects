namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class LocationSeeder : DataSeeder, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Locations.Any())
            {
                return;
            }

            var locations = this.GetDataFromJson<Location>(nameof(Location), serviceProvider);

            await AddDataAsync(dbContext, locations);
        }

        private static async Task AddDataAsync(ApplicationDbContext dbContext, IEnumerable<Location> locations)
        {
            foreach (var location in locations)
            {
                var dbLocation = new Location
                {
                    Name = location.Name,
                };

                foreach (var populatedPlace in location.PopulatedPlaces)
                {
                    var dbPopulatedPLace = new PopulatedPlace
                    {
                        Name = populatedPlace.Name,
                        Location = dbLocation,
                    };
                    await dbContext.PopulatedPlaces.AddAsync(dbPopulatedPLace);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
