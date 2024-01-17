namespace RealEstate.Services.Data.Tests.Mocks
{
    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;

    internal class UserContactRepositoryMock
    {
        internal static IDeletableEntityRepository<UserContact> Instance
        {
            get
            {
                return new Mock<IDeletableEntityRepository<UserContact>>().Object;
            }
        }
    }
}
