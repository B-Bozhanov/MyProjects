namespace RealEstate.Web.ViewModels.Locations
{
    using System.Collections.Generic;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.PopulatedPlaces;

    public class LocationViewModel : IMapFrom<Location>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<PopulatedPlaceViewModel> Places { get; set; }
    }
}
