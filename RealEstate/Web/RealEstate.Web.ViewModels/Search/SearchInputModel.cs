namespace RealEstate.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public class SearchInputModel : BasePropertyModel
    {
        public int Id { get; init; }

        #nullable enable
        public string? KeyWord { get; set; }

        public PropertyOptionModel? Type { get; set; }

        public int? BedRooms { get; set; }

        public int? Garages { get; set; }

        public int? BathRooms { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int LocationId { get ; set ; }

        public int PopulatedPlaceId { get ; set ; }

        public int PropertyTypeId { get; set; }

        public IEnumerable<PropertyTypeViewModel>? PropertyTypes { get; set; }

        public IEnumerable<LocationViewModel>? PopulatedPlaces { get; init; }

        public IEnumerable<LocationViewModel>? Locations { get; set; }
    }
}
