namespace RealEstate.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Models;

    public interface IImageService
    {
        public Task AddAsync(IFormFileCollection images, Property property);
    }
}
