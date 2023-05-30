namespace RealEstate.Services.Data
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats;
    using SixLabors.ImageSharp.Formats.Png;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;

    using static RealEstate.Common.GlobalConstants.Images;

    public class ImageService : IImageService
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IDeletableEntityRepository<RealEstate.Data.Models.Image> imageRepository;

        public ImageService(IHostingEnvironment hostingEnvironment, IDeletableEntityRepository<RealEstate.Data.Models.Image> imageRepository)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.imageRepository = imageRepository;
        }

        public async Task Save(IFormFileCollection images, Property property)
        {
            var rootPath = this.hostingEnvironment.WebRootPath;
            var imagePath = $"{rootPath}{ImagePath}";

            foreach (var image in images)
            {
                var imageExtencion = new FileInfo(image.FileName).Extension;
                var imageName = $"{Guid.NewGuid()}.{imageExtencion}";

                var dbImage = new RealEstate.Data.Models.Image { Name = imageName };

                dbImage.Property = property;

                var memoryStream = new MemoryStream();

                await image.CopyToAsync(memoryStream);

                var imageToResize = SixLabors.ImageSharp.Image.Load(new ReadOnlySpan<byte>(memoryStream.ToArray()));

                imageToResize.Mutate(x => x.Resize(600, 800, KnownResamplers.Lanczos3));

                using var fileStream = new FileStream($"{imagePath}{imageName}", FileMode.Create);

                imageToResize.Save(fileStream, imageToResize.Metadata.DecodedImageFormat);//Replace Png encoder with the file format of choice

                await this.imageRepository.AddAsync(dbImage);
            }
        }
    }
}
