namespace RealEstate.Services.Data.Tests.Mocks
{
    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;

    internal static class ConditionRepositoryMock
    {
        internal static IDeletableEntityRepository<Condition> Instance
        {
            get
            {
                return new Mock<IDeletableEntityRepository<Condition>>().Object;
            }
        }
    }
}
