namespace RealEstate.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.Property;

    public class SearchInputModel : BasePropertyModel
    {
        public string? KeyWord { get; set; }

        //TODO: Use another enum, not database enum
        public PropertyOptionModel? Type { get; set; }

        public int? BedRooms { get; set; }

        public int? Garages { get; set; }

        public int? BathRooms { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public IEnumerable<LocationViewModel> PopulatedPlaces { get; init; }
    }
}
