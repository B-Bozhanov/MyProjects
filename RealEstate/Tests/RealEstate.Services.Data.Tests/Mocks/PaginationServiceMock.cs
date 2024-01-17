namespace RealEstate.Services.Data.Tests.Mocks
{
    using Moq;

    using RealEstate.Services.Interfaces;

    internal class PaginationServiceMock
    {
        internal static IPaginationService Instance
        {
            get
            {
                return new Mock<IPaginationService>().Object;
            }
        }
    }
}
