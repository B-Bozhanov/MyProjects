namespace RealEstate.Services.Data.Tests.Mocks
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;

    internal static class PropertyTypeRepositoryMock
    {
        internal static IDeletableEntityRepository<PropertyType> Instance
        {
            get
            {
                var propertyTypeRepositoriMock = new Mock<IDeletableEntityRepository<PropertyType>>();
                var propertyTypes = new List<PropertyType>();

                for (int i = 1; i <= 10; i++)
                {
                    propertyTypes.Add(new PropertyType { Id = i, Name = $"PropertyType{i.ToString()}" });
                }

                propertyTypeRepositoriMock
                    .Setup(pt => pt.All())
                    .Returns(propertyTypes.AsQueryable);

                return propertyTypeRepositoriMock.Object;
            }
        }
    }
}
