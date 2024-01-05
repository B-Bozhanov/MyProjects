namespace RealEstate.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    using Hangfire;
    using Hangfire.Common;
    using Hangfire.States;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    using RealEstate.Data;
    using RealEstate.Data.Models;
    using RealEstate.Data.Repositories;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels;
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

            var propertyService = await this.GetPropertyServiceInstance(model, dbContext);

            Assert.Equal(0, await dbContext.Properties.CountAsync());

            await propertyService.AddAsync(model, new ApplicationUser());
            Assert.Equal(1, await dbContext.Properties.CountAsync());

            await propertyService.AddAsync(model, new ApplicationUser());
            await propertyService.AddAsync(model, new ApplicationUser());
            Assert.Equal(3, await dbContext.Properties.CountAsync());
        }

        [Fact]
        public async Task GetByIdShouldReturnCorectProperty()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "PropertyTestDb").Options;
            using var dbContext = new ApplicationDbContext(options);

            var model = new PropertyInputModel
            {
                Id = 12,
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

            var propertyService = await this.GetPropertyServiceInstance(model, dbContext);

            var result = await propertyService.GetByIdAsync<PropertyInputModel>(12);

            Assert.Equal(12, result.Id);
        }

        private async Task<PropertyService> GetPropertyServiceInstance(PropertyInputModel model, ApplicationDbContext dbContext)
        {
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
            var services = new Mock<IServiceCollection>();

            var hangfireWrapper = new Mock<IHangfireWrapperService>();
            hangfireWrapper.Setup(x => x.BackgroundJobClient.Create(It.IsAny<Job>(), It.IsAny<EnqueuedState>()));
                        
            await populatedPlaceRepository.AddAsync(new PopulatedPlace { Id = 1, Name = "Test1" });
            await propertyTypeRepository.AddAsync(new PropertyType { Id = 1, Name = "Type" });
            await buildingTypeRepository.AddAsync(new BuildingType { Id = 1, Name = "BuildingType" });

            await populatedPlaceRepository.SaveChangesAsync();
            await propertyTypeRepository.SaveChangesAsync();
            await buildingTypeRepository.SaveChangesAsync();


            var propertyService = new PropertyService(
                propertyRepository,
                propertyTypeRepository,
                buildingTypeRepository,
                userContactsRepository,
                populatedPlaceRepository,
                conditionRepository,
                detailRepository,
                equipmentRepository,
                heatingRepository, imageService, hangfireWrapper.Object);

            return propertyService;
        }
    }
}
