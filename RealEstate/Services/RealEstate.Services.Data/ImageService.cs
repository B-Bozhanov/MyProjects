namespace RealEstate.Services.Data
{
    using System;
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

        public ImageService(IHostingEnvironment hostingEnvironment, IDeletableEntityRepository<RealEstate.Data.Models.Image> imageRepository)
        {
            this.imageRepository = imageRepository;
            this.rootPath = hostingEnvironment.WebRootPath;
        }

        public async Task Add(IFormFileCollection images, RealEstate.Data.Models.Property property)
        {
            if (images != null)
            {
                foreach (var image in images)
                {
                    var imageExtencion = new FileInfo(image.FileName).Extension;
                    var imageName = $"{Guid.NewGuid()}.{imageExtencion}";

                    var dbImage = new RealEstate.Data.Models.Image { Name = imageName };
                    dbImage.Property = property;

                    await this.imageRepository.AddAsync(dbImage);
                    await this.Save(image, imageName);
                }
            }
        }

        private async Task Save(IFormFile image, string imageName)
        {
            var imagePath = $"{this.rootPath}{ImagePath}";

            var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            using var fileStream = new FileStream($"{imagePath}{imageName}", FileMode.Create);

            var newImage = this.Resize(memoryStream);
            newImage.Save(fileStream, ImageFormat.Png);
        }

        private System.Drawing.Image Resize(Stream stream)
        {
            var imageToResize = Image.FromStream(stream, true, true);

            // Image image = imageToResize.GetThumbnailImage(Width, (Width * imageToResize.Height) / imageToResize.Width, null, IntPtr.Zero);
            var image = (Image)new Bitmap(imageToResize, new System.Drawing.Size(Width, Height));

            return image;
        }

        //public System.Drawing.Image resizeImage(int newWidth, int newHeight, string stPhotoPath)
        //{
        //    System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(stPhotoPath);

        //    int sourceWidth = imgPhoto.Width;
        //    int sourceHeight = imgPhoto.Height;

        //    if (sourceWidth < sourceHeight)
        //    {
        //        int buff = newWidth;

        //        newWidth = newHeight;
        //        newHeight = buff;
        //    }

        //    int sourceX = 0, sourceY = 0, destX = 0, destY = 0;

        //    float nPercent = 0, nPercentW = 0, nPercentH = 0;

        //    nPercentW = ((float)newWidth / (float)sourceWidth);
        //    nPercentH = ((float)newHeight / (float)sourceHeight);
        //    if (nPercentH < nPercentW)
        //    {
        //        nPercent = nPercentH;
        //        destX = System.Convert.ToInt16((newWidth -
        //                  (sourceWidth * nPercent)) / 2);
        //    }
        //    else
        //    {
        //        nPercent = nPercentW;
        //        destY = System.Convert.ToInt16((newHeight -
        //                  (sourceHeight * nPercent)) / 2);
        //    }

        //    int destWidth = (int)(sourceWidth * nPercent);
        //    int destHeight = (int)(sourceHeight * nPercent);


        //    Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
        //                  PixelFormat.Format24bppRgb);

        //    bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
        //                 imgPhoto.VerticalResolution);

        //    Graphics grPhoto = Graphics.FromImage(bmPhoto);
        //    grPhoto.Clear(Color.Black);
        //    grPhoto.InterpolationMode =
        //        System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        //    grPhoto.DrawImage(imgPhoto,
        //        new Rectangle(destX, destY, destWidth, destHeight),
        //        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
        //        GraphicsUnit.Pixel);

        //    grPhoto.Dispose();
        //    imgPhoto.Dispose();
        //    return bmPhoto;
        //}
    }
}
