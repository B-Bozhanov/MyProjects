namespace RealEstate.Services.Data.Tests.Mocks
{
    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Data.Repositories;

    internal static class PropertyRepositoryMock
    {
        internal static IDeletableEntityRepository<Property> Instance
        {
            get
            {
                var dbContext = DbContextMock.Instance;
                var propertyRepository = new EfDeletableEntityRepository<Property>(dbContext);

                return propertyRepository;
            }
        }
    }
}
