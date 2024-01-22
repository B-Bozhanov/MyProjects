namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.ImageModel;

    public interface IImageService
    {
        public Task AddAsync(IFormFileCollection images, Property property, bool SaveToLocalDrive);

        public Task<ICollection<ImageViewModel>> GetAllByPropertyIdAsync(int propertyId);
    }
}
