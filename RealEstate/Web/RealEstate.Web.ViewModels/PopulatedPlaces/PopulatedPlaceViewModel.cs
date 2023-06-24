namespace RealEstate.Web.ViewModels.PopulatedPlaces
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Locations;

    public class PopulatedPlaceViewModel : IMapFrom<PopulatedPlace>
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public LocationViewModel Location { get; init; }
    }
}
