namespace RealEstate.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.Property;

    public class SearchInputModel
    {
        public string? KeyWord { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Display(Name = "Populated place")]
        public int? PopulatedPlaceId { get; set; }

        //TODO: Use another enum, not database enum
        public PropertyOptionModel? Type { get; set; }

        public int? BedRooms { get; set; }

        public int? Garages { get; set; }

        public int? BathRooms { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public IEnumerable<LocationViewModel> Locations { get; set; }

        public IEnumerable<LocationViewModel> PopulatedPlaces { get; init; }
    }
}
