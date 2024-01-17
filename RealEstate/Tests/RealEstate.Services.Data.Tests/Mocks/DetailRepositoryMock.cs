namespace RealEstate.Services.Data.Tests.Mocks
{
    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;

    internal static class DetailRepositoryMock
    {
        internal static IDeletableEntityRepository<Detail> Instance
        {
            get
            {
                return new Mock<IDeletableEntityRepository<Detail>>().Object;
            }
        }
    }
}
