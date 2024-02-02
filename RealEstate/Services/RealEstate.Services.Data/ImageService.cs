namespace RealEstate.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using Newtonsoft.Json;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.ImageModel;

    using RestSharp;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.ColorSpaces;
    using SixLabors.ImageSharp.Formats.Jpeg;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;

    using static RealEstate.Common.GlobalConstants;

    using DbImage = RealEstate.Data.Models.Image;
    using DbProperty = RealEstate.Data.Models.Property;
    using IshImage = SixLabors.ImageSharp.Image;

    public class ImageService : IImageService
    {
        private readonly IConfiguration configuration;
        private readonly IDeletableEntityRepository<DbImage> imageRepository;
        private readonly string rootPath;

        public ImageService(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IDeletableEntityRepository<DbImage> imageRepository)
        {
            this.configuration = configuration;
            this.imageRepository = imageRepository;
            this.rootPath = hostingEnvironment.WebRootPath;
        }

        public async Task AddAsync(IFormFileCollection files, DbProperty property, bool SaveToLocalDrive)
        { 
            if (files != null)
            {
                foreach (var file in files)
                {
                    this.Validator(file);

                    var dbImage = new DbImage();
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

        public async Task<byte[]> GetImageBytesAsync(IshImage image)
        {
            using var imageStream = new MemoryStream();
            await image.SaveAsJpegAsync(imageStream);
            return imageStream.ToArray();
        }

        public async Task<MemoryStream> GetImageStreamAsync(IFormFile image)
        {
            using var imageStream = new MemoryStream();
            await image.CopyToAsync(imageStream);

            return imageStream;
        }

        public async Task SaveToLocalDriveAsync(IFormFile file, DbImage dbImage)
        {
            Directory.CreateDirectory($"{this.rootPath}{Images.PropertyImagesUrl}");

            var imageExtencion = new FileInfo(file.FileName).Extension;
            var imageFullUrl = $"{this.rootPath}{Images.PropertyImagesUrl}{dbImage.Id}.{imageExtencion}";
            dbImage.Url = $"{Images.PropertyImagesUrl}{dbImage.Id}.{imageExtencion}";

            using var fileStream = new FileStream($"{imageFullUrl}", FileMode.Create);

            var editedImage = await this.Resize(file);
            await editedImage.SaveAsJpegAsync(fileStream);
        }

        public async Task<RestResponse> SaveToRemoteCloudAsync(IFormFile file)
        {
            var resizedImage = await this.Resize(file);
            var imgStringBase64 = Convert.ToBase64String(await this.GetImageBytesAsync(resizedImage));
            var imgApiKey = this.configuration["ApiKeys:ImgBBApiKey"];

            var client = new RestClient($"{Images.UploadUrl}&key={imgApiKey}");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddParameter("key", imgApiKey);
            request.AddParameter("image", imgStringBase64);
            request.AddParameter("name", $"{file.FileName}");

            return await client.PostAsync(request);
        }

        public async Task<IshImage> Resize(IFormFile file)
        {
            var imageToResize = await IshImage.LoadAsync(file.OpenReadStream());
            imageToResize.Mutate(x => x.Resize(new ResizeOptions() { Mode = ResizeMode.Crop, Size = new Size(Images.Width, Images.Height)}));
            return imageToResize;
        }

        private void Validator(IFormFile file)
        {
            var supportedImageExtensions = Images.GetSupportedExtensions(); 
            var fileExtension = new FileInfo(file.FileName).Extension;
            using var image = IshImage.Load(file.OpenReadStream());

            if (image.Width < Images.Width || image.Height < Images.Height)
            {
                throw new InvalidDataException($"The image width must be more than {Images.Width}, and the image height must bemore than {Images.Height}!");
            }

            if (!supportedImageExtensions.Contains(fileExtension))
            {
                throw new InvalidDataException("Invalid image extension");
            }

            if (file.Length > Images.MaxImageSize)
            {
                throw new InvalidDataException("The max size is 10MB.");
            }
        }
    }
}
