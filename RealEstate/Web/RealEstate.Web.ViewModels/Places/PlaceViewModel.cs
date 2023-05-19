namespace RealEstate.Web.ViewModels.Places
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class PlaceViewModel : IMapFrom<Place>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
