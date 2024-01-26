namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RealEstate.Data.Models;

    internal class EquipmentSeeder : DataSeeder, ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Equipment.Any())
            {
                return;
            }

            var equipments = this.GetDataFromJson<Equipment>(nameof(Equipment), serviceProvider);
            await AddDataAsync(dbContext, equipments);
        }

        private static async Task AddDataAsync(ApplicationDbContext dbContext, IEnumerable<Equipment> equipments)
        {
            foreach (var equipment in equipments)
            {
                await dbContext.Equipment.AddAsync(equipment);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
