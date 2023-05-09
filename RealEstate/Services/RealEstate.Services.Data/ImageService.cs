namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;

    public class ImageService : IImageService
    {
        public ICollection<Image> GetImages(IFormFileCollection files)
        {
            var images = new List<Image>();

            if (files != null)
            {
                foreach (var file in files)
                {
                    var stream = new MemoryStream();
                    file.CopyTo(stream);

                    var image = new Image { Name = file.Name }; // Content = stream.ToArray() }

                    images.Add(image);
                }
            }

            return images;
        }
    }
}
