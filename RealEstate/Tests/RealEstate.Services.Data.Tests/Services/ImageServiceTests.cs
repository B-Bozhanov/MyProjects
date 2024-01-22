namespace RealEstate.Services.Data.Tests.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Tests.Mocks;

    using Xunit;

    using static RealEstate.Common.GlobalConstants;

    public class ImageServiceTests
    {
        [Fact]
        public async Task ImageValidatorShouldThrowAnExceptionIfFileSizeIsMoreThan10MB()
        {
            var memoryStream = new MemoryStream(new byte[500]);

            var imageService = new ImageService(new Mock<IWebHostEnvironment>().Object, new Mock<IConfiguration>().Object, new Mock<IDeletableEntityRepository<Image>>().Object);
            var fileCollection = new FormFileCollection();

            IFormFile file = new FormFile(memoryStream, memoryStream.Length, 1024 * 1024 * 11, "test", $"test.jpg");

            fileCollection.Add(file);

            await Assert.ThrowsAsync<InvalidDataException>(async () => await imageService.AddAsync(fileCollection, new Property(), false));
        }

        [Fact]
        public async Task ImageValidatorShouldThrowAnExceptionIfFileExtensionIsNotSupported()
        {
            var memoryStream = new MemoryStream(new byte[500]);

            var imageService = new ImageService(new Mock<IWebHostEnvironment>().Object, new Mock<IConfiguration>().Object, new Mock<IDeletableEntityRepository<Image>>().Object);
            var fileCollection = new FormFileCollection();

            IFormFile file = new FormFile(memoryStream, memoryStream.Length, 1024 * 1024 * 5, "test", $"test.mp4");

            fileCollection.Add(file);

            await Assert.ThrowsAsync<InvalidDataException>(async () => await imageService.AddAsync(fileCollection, new Property(), false));
        }
    }
}
