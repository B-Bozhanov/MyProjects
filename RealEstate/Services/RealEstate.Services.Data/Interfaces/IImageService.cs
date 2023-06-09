namespace RealEstate.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Models;

    public interface IImageService
    {
        public Task Add(IFormFileCollection images, Property property);
    }
}
