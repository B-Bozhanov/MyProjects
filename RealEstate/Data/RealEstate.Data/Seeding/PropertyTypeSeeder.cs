namespace RealEstate.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class PropertyTypeSeeder : DataSeeder, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PropertyTypes.Any())
            {
                return;
            }

            var propertyTypes = this.GetData<PropertyType>(nameof(PropertyType));

            await dbContext.PropertyTypes.AddRangeAsync(propertyTypes);
        }
    }
}
