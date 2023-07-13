namespace RealEstate.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hangfire;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using RealEstate.Data;
    using RealEstate.Data.Models;
    using RealEstate.Data.Repositories;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.ContactModel;
    using RealEstate.Web.ViewModels.Property;

    using Xunit;

    public class PropertyServiceTests
    {
        [Fact]
        public async Task AddAsyncShouldAddTheCorectPropertyInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "PropertyTestDb").Options;
            using var dbContext = new ApplicationDbContext(options);

            var propertyRepository = new EfDeletableEntityRepository<Property>(dbContext);
            var propertyTypeRepository = new EfDeletableEntityRepository<PropertyType>(dbContext);
            var buildingTypeRepository = new EfDeletableEntityRepository<BuildingType>(dbContext);
            var userContactsRepository = new EfDeletableEntityRepository<UserContact>(dbContext);
            var populatedPlaceRepository = new EfDeletableEntityRepository<PopulatedPlace>(dbContext);
            var conditionRepository = new EfDeletableEntityRepository<Condition>(dbContext);
            var detailRepository = new EfDeletableEntityRepository<Detail>(dbContext);
            var equipmentRepository = new EfDeletableEntityRepository<Equipment>(dbContext);
            var heatingRepository = new EfDeletableEntityRepository<Heating>(dbContext);
            var imageRepository = new EfDeletableEntityRepository<Image>(dbContext);
            var test = new Mock<IWebHostEnvironment>();

            var imageService = new ImageService(test.Object, imageRepository);

            var backgroundJobClient = new Mock<IBackgroundJobClient>();

            await populatedPlaceRepository.AddAsync(new PopulatedPlace { Id = 1, Name = "Test1" });
            await propertyTypeRepository.AddAsync(new PropertyType { Id = 1, Name = "Type" });
            await buildingTypeRepository.AddAsync(new BuildingType { Id = 1, Name = "BuildingType" });

            await populatedPlaceRepository.SaveChangesAsync();
            await propertyTypeRepository.SaveChangesAsync();
            await buildingTypeRepository.SaveChangesAsync();

            var model = new PropertyInputModel
            {
                PopulatedPlaceId = 1,
                PropertyTypeId = 1,
                Option = PropertyOption.Sale,
                BuildingTypes = new List<BuildingTypeViewModel>(),
                ContactModel = new ContactModel
                {
                    Email = "bojanilkov88@gmail.com",
                    PhoneNumber = "0896655707",
                    Names = "Bozhan",
                },
            };

            var propertyService = new PropertyService(
                propertyRepository, 
                propertyTypeRepository, 
                buildingTypeRepository, 
                userContactsRepository, 
                populatedPlaceRepository, 
                conditionRepository, 
                detailRepository, 
                equipmentRepository, 
                heatingRepository, imageService, backgroundJobClient.Object);

            await propertyService.AddAsync(model, new ApplicationUser());

            Assert.Equal(1, await dbContext.Properties.CountAsync());
        }

        [Fact]
        public async Task GetAllCountShouldWorkCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "PropertyTestDb").Options;
            using var dbContext = new ApplicationDbContext(options);

            var propertyService = new Mock<IPropertyService>();
        }
    }
}
