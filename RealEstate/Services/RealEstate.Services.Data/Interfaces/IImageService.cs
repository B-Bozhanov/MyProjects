namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using RealEstate.Data.Models;

    public interface IImageService
    {
        ICollection<Image> GetImages(IFormFileCollection files);
    }
}
