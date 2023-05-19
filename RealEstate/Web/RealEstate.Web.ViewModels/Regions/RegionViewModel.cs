namespace RealEstate.Web.ViewModels.Regions
{
    using System.Collections.Generic;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.DownTowns;
    using RealEstate.Web.ViewModels.Places;

    public class RegionViewModel : IMapFrom<Region>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DownTownViewModel DownTown { get; set; }

        public ICollection<PlaceViewModel> Places { get; set; }
    }
}
