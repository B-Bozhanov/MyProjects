namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.ImageModel;

    using RestSharp;

    using IshImage = SixLabors.ImageSharp.Image;

    public interface IImageService
    {
        public Task AddAsync(IFormFileCollection images, Property property, bool SaveToLocalDrive);

        public Task<ICollection<ImageViewModel>> GetAllByPropertyIdAsync(int propertyId);

        public Task<byte[]> GetImageBytesAsync(IshImage image);

        public Task<MemoryStream> GetImageStreamAsync(IFormFile image);

        public Task SaveToLocalDriveAsync(IFormFile file, Image dbImage);

        public Task<RestResponse> SaveToRemoteCloudAsync(IFormFile file);

        public Task<IshImage> Resize(IFormFile file);
    }
}
