namespace RealEstate.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    using RestSharp;

    using static System.Net.Mime.MediaTypeNames;
    using static RealEstate.Common.GlobalConstants;
    using RealEstate.Web.ViewModels.ImageModel;

    public class ImageService : IImageService
    {
        private readonly IConfiguration configuration;
        private readonly IDeletableEntityRepository<RealEstate.Data.Models.Image> imageRepository;
        private readonly string rootPath;

        public ImageService(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IDeletableEntityRepository<RealEstate.Data.Models.Image> imageRepository)
        {
            this.configuration = configuration;
            this.imageRepository = imageRepository;
            this.rootPath = hostingEnvironment.WebRootPath;
        }

        public async Task AddAsync(IFormFileCollection files, RealEstate.Data.Models.Property property, bool SaveToLocalDrive)
        { 
            if (files != null)
            {
                foreach (var file in files)
                {
                    this.Validator(file);

                    var dbImage = new RealEstate.Data.Models.Image();
                    dbImage.Property = property;

                    if (SaveToLocalDrive)
                    {
                        await this.SaveToLocalDriveAsync(file, dbImage);
                    }
                    else
                    {
                        try
                        {
                            var response = await this.SaveToRemoteCloudAsync(file);

                            var imageViewModel = JsonConvert.DeserializeObject<ImageViewModel>(response.Content);

                            dbImage.Url = imageViewModel.Data.Url;
                            dbImage.DeleteUrl = imageViewModel.Data.Delete_Url;
                            dbImage.Size = file.Length;
                            dbImage.Extension = new FileInfo(file.FileName).Extension;
                            dbImage.Width = imageViewModel.Data.Width;
                            dbImage.Height = imageViewModel.Data.Height;
                            dbImage.Expiration = imageViewModel.Data.Expiration;
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(ex.Message);
                        }
                    }

                    await this.imageRepository.AddAsync(dbImage);
                }
            }
        }

        public async Task<ICollection<ImageViewModel>> GetAllByPropertyIdAsync(int propertyId) =>
            await this.imageRepository
            .All()
            .Where(i => i.PropertyId == propertyId)
            .To<ImageViewModel>()
            .ToListAsync();

        private async Task<byte[]> GetImageBytesAsync(IFormFile image)
        {
            var imageStream = new MemoryStream();
            await image.CopyToAsync(imageStream);

            return imageStream.ToArray();
        }

        private async Task<MemoryStream> GetImageStreamAsync(IFormFile image)
        {
            var imageStream = new MemoryStream();
            await image.CopyToAsync(imageStream);

            return imageStream;
        }

        private async Task SaveToLocalDriveAsync(IFormFile image, RealEstate.Data.Models.Image dbImage)
        {
            var imageExtencion = new FileInfo(image.FileName).Extension;
            var imageFullUrl = $"{this.rootPath}{Images.PropertyImagesUrl}{dbImage.Id}.{imageExtencion}";
            dbImage.Url = $"{Images.PropertyImagesUrl}{dbImage.Id}.{imageExtencion}";

            using var fileStream = new FileStream($"{imageFullUrl}", FileMode.Create);
            
            var editedImage = this.Resize(await this.GetImageStreamAsync(image));
            editedImage.Save(fileStream, ImageFormat.Jpeg);
        }

        private async Task<RestResponse> SaveToRemoteCloudAsync(IFormFile image)
        {
            var imgStringBase64 = Convert.ToBase64String(await this.GetImageBytesAsync(image));
            var imgApiKey = this.configuration["ApiKeys:ImgBBApiKey"];

            var client = new RestClient($"{Images.UploadUrl}&key={imgApiKey}");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddParameter("key", imgApiKey);
            request.AddParameter("image", imgStringBase64);
            request.AddParameter("name", $"{image.FileName}");

            return await client.PostAsync(request);
        }

        private System.Drawing.Image Resize(Stream imageStream)
        {
            var imageToResize = System.Drawing.Image.FromStream(imageStream, true, true);
            var image = (System.Drawing.Image)new Bitmap(imageToResize, new System.Drawing.Size(Images.Width, Images.Height));
            return image;
        }

        private void Validator(IFormFile file)
        {
            var supportedImageExtensions = Images.GetSupportedExtensions(); 
            var fileExtension = new FileInfo(file.FileName).Extension;

            if (!supportedImageExtensions.Contains(fileExtension))
            {
                throw new InvalidDataException("Invalid image extension");
            }

            if (file.Length > Images.ImageMaxSize)
            {
                throw new InvalidDataException("The max size is 10MB.");
            }
        }
    }
}
