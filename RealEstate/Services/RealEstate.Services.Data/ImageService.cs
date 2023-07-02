namespace RealEstate.Services.Data
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Services.Data.Interfaces;

    using static RealEstate.Common.GlobalConstants.Images;

    public class ImageService : IImageService
    {
        private readonly IDeletableEntityRepository<RealEstate.Data.Models.Image> imageRepository;
        private readonly string rootPath;

        public ImageService(IWebHostEnvironment hostingEnvironment, IDeletableEntityRepository<RealEstate.Data.Models.Image> imageRepository)
        {
            this.imageRepository = imageRepository;
            this.rootPath = hostingEnvironment.WebRootPath;
        }

        public async Task AddAsync(IFormFileCollection images, RealEstate.Data.Models.Property property)
        {
            if (images != null)
            {
                foreach (var image in images)
                {
                    var dbImage = new RealEstate.Data.Models.Image();

                    var imageExtencion = new FileInfo(image.FileName).Extension;
                    var imageUrl = $"{dbImage.Id}.{imageExtencion}";

                    dbImage.Property = property;
                    dbImage.Url = imageUrl;

                    await this.imageRepository.AddAsync(dbImage);
                    await this.SaveToLocalDriveAsync(image, imageUrl);
                }
            }
        }

        private async Task SaveToLocalDriveAsync(IFormFile image, string imageUrl)
        {
            var imagePath = $"{this.rootPath}{PropertyImagesPath}";

            var imageStream = new MemoryStream();
            await image.CopyToAsync(imageStream);

            using var fileStream = new FileStream($"{imagePath}{imageUrl}", FileMode.Create);

            var editedImage = this.Resize(imageStream);
            editedImage.Save(fileStream, ImageFormat.Jpeg);
        }

        private Image Resize(Stream imageStream)
        {
            var imageToResize = Image.FromStream(imageStream, true, true);

            var image = (Image)new Bitmap(imageToResize, new System.Drawing.Size(Width, Height));

            return image;
        }
    }
}
