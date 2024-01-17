namespace RealEstate.Services.Data.Tests.Mocks
{
    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;

    internal static class HeatingRepositoryMock
    {
        internal static IDeletableEntityRepository<Heating> Instance
        {
            get
            {
                return new Mock<IDeletableEntityRepository<Heating>>().Object;
            }
        }
    }
}
