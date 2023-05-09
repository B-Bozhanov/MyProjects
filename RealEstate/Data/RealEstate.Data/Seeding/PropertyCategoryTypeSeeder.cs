namespace RealEstate.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class PropertyCategoryTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PropertyCategoryTypes.Any())
            {
                return;
            }

            await dbContext.PropertyCategoryTypes.AddAsync(new PropertyCategoryType { Id = 1, Name = "Жилищни имоти" });
            await dbContext.PropertyCategoryTypes.AddAsync(new PropertyCategoryType { Id = 2, Name = "Бизнес имоти" });
            await dbContext.PropertyCategoryTypes.AddAsync(new PropertyCategoryType { Id = 3, Name = "Земеделска земя" });
            await dbContext.PropertyCategoryTypes.AddAsync(new PropertyCategoryType { Id = 4, Name = "Други" });
            await dbContext.PropertyCategoryTypes.AddAsync(new PropertyCategoryType { Id = 5, Name = "Всички" });
        }
    }
}
