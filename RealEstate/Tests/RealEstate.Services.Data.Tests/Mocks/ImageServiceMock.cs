namespace RealEstate.Services.Data.Tests.Mocks
{
    using Moq;

    using RealEstate.Services.Data.Interfaces;

    internal static  class ImageServiceMock
    {
        internal static IImageService Instance
        {
            get
            {
                return new Mock<IImageService>().Object;
            }
        }
    }
}
