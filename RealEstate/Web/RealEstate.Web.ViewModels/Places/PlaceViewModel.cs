namespace RealEstate.Web.ViewModels.Places
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class PlaceViewModel : IMapFrom<PopulatedPlace>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
