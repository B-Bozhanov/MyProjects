namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    internal class PropertyTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.PropertyTypes.Any())
            {
                return;
            }

            var propertyTypes = new List<PropertyType>
            {
                 new PropertyType
                 {
                     Name = "Едностаен апартамент",
                     PropertyCategoryTypeId = 1,
                 },
                 new PropertyType
                {
                    Name = "Двустаен апартамент",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Тристаен апартамент",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Четиристаен апартамент",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Многостаен",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Мезонет",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Ателие/Студио",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Къща",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Етаж от Къща",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Вила",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Жилищна сграда",
                    PropertyCategoryTypeId = 1,
                },
                 new PropertyType
                {
                    Name = "Офис",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Магазин",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Търговско помещение",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Медицински кабинет",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Заведение",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Склад",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Гараж",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Парко място",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Хотел",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Бизнес сграда",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Промишлени терени",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Производствена сграда",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Бензиностанция / Газстанция",
                    PropertyCategoryTypeId = 2,
                },
                 new PropertyType
                {
                    Name = "Нива",
                    PropertyCategoryTypeId = 3,
                },
                 new PropertyType
                {
                    Name = "Гора",
                    PropertyCategoryTypeId = 3,
                },
                 new PropertyType
                {
                    Name = "Лозе",
                    PropertyCategoryTypeId = 3,
                },
                 new PropertyType
                {
                    Name = "Овощна градина",
                    PropertyCategoryTypeId = 3,
                },
                 new PropertyType
                {
                    Name = "Зеленчукова градина",
                    PropertyCategoryTypeId = 3,
                },
                 new PropertyType
                {
                    Name = "Други",
                    PropertyCategoryTypeId = 4,
                },
                 new PropertyType
                {
                    Name = "Всички",
                    PropertyCategoryTypeId = 5,
                },
            };

            await dbContext.PropertyTypes.AddRangeAsync(propertyTypes);
        }
    }
}
