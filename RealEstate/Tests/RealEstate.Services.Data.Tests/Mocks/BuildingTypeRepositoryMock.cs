namespace RealEstate.Services.Data.Tests.Mocks
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;

    public static class BuildingTypeRepositoryMock
    {
        public static IDeletableEntityRepository<BuildingType> Instance
        {
            get
            {
                var buildingTypeRepositoryMock = new Mock<IDeletableEntityRepository<BuildingType>>();
                var buildingTypes = new List<BuildingType>();

                for (int i = 1; i <= 10; i++)
                {
                    buildingTypes.Add(new BuildingType { Id = i, Name = $"BuildingType{i.ToString()}" });
                }

                buildingTypeRepositoryMock
                    .Setup(bt => bt.All())
                    .Returns(buildingTypes.AsQueryable);

                return buildingTypeRepositoryMock.Object;
            }
        }
    }
}
