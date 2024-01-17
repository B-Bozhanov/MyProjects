namespace RealEstate.Services.Data.Tests.Mocks
{
    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;

    internal static class EquipmentRepositoryMock
    {
        internal static IDeletableEntityRepository<Equipment> Instance
        {
            get
            {
                return new Mock<IDeletableEntityRepository<Equipment>>().Object;
            }
        }
    }
}
