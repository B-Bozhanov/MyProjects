namespace RealEstate.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class PropertyTypeSeeder : DataSeederBase, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PropertyTypes.Any())
            {
                return;
            }

            var propertyTypes = this.GetDataFromJson<PropertyType>(nameof(PropertyType), serviceProvider);

            await dbContext.PropertyTypes.AddRangeAsync(propertyTypes);
        }
    }
}
