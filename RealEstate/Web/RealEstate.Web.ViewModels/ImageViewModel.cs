namespace RealEstate.Web.ViewModels
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class ImageViewModel : IMapFrom<Image>
    {
        public string Id { get; set; }

        public string Url { get; set; }
    }
}
