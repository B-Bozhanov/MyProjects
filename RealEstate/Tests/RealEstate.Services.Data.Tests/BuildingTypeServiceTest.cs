namespace RealEstate.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using RealEstate.Data;
    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Data.Repositories;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels;
    using RealEstate.Web.ViewModels.BuildingTypeModel;

    using Xunit;

    public class BuildingTypeServiceTest
    {
        [Fact]
        public void GetAllShouldWorkCorectly()
        {
            var repository = new Mock<IDeletableEntityRepository<BuildingType>>();

            repository.Setup(x => x.All()).Returns(new List<BuildingType> 
                                                   { 
                                                       new BuildingType(), 
                                                       new BuildingType(), 
                                                       new BuildingType(), 
                                                   }.AsQueryable());

            var service = new BuildingTypeService(repository.Object);

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var result = service.GetAll();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetAllShouldReturnBuildingTypeViewModel()
        {
            var repository = new Mock<IDeletableEntityRepository<BuildingType>>();

            repository.Setup(x => x.All()).Returns(new List<BuildingType>
                                                   {
                                                       new BuildingType(),
                                                   }.AsQueryable());

            var service = new BuildingTypeService(repository.Object);

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            var result = service.GetAll();

            Assert.Equal(typeof(BuildingTypeViewModel), result.First().GetType());
        }
    }
}
