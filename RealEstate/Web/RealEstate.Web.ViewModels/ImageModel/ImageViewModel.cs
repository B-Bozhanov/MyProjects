namespace RealEstate.Web.ViewModels.ImageModel
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class ImageViewModel : IMapFrom<Image>
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public long Size { get; set; }

        public int Expiration { get; set; }

        public string Extension { get; set; }

        public ImageData Data { get; set; }
    }
}
