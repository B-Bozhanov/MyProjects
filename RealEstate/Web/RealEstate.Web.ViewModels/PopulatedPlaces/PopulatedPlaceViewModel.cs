namespace RealEstate.Web.ViewModels.PopulatedPlaces
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class PopulatedPlaceViewModel : IMapFrom<PopulatedPlace>
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string LocationName { get; init; }
    }
}
