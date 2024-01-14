namespace RealEstate.Services.Data.Tests.Mocks
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;

    internal static class PopulatedPlaceRepositoryMock
    {
        public static IDeletableEntityRepository<PopulatedPlace> Instance
        {
            get
            {
                var populatedPlaceRepositoryMock = new Mock<IDeletableEntityRepository<PopulatedPlace>>();
                var populatedPlaces = new List<PopulatedPlace>();

                for (int i = 1; i <= 10; i++)
                {
                    populatedPlaces.Add(new PopulatedPlace { Id = i, Name = $"BuildingType1{i.ToString()}" });
                }

                populatedPlaceRepositoryMock
                    .Setup(bt => bt.All())
                    .Returns(populatedPlaces.AsQueryable);

                return populatedPlaceRepositoryMock.Object;
            }
        }
    }
}
