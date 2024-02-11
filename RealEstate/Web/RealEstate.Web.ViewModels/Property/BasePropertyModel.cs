namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public abstract class BasePropertyModel : IMapFrom<Property>
    {
        public int Id { get; init; }

        [Required]
        public string Description { get; init; }

        public int ExpirationDays { get; set; }
        public PopulatedPlaceViewModel PopulatedPlace { get; set; }
        public PropertyOptionModel Option { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The price can not be negative")]
        public decimal Price { get; init; }

        [Required(ErrorMessage = "The field is required!")]
        public int Size { get; init; }

        public int? YardSize { get; set; }

        public int Floor { get; set; }

        public int TotalFloors { get; set; }

        public int? Year { get; set; }

        [Required]
        public int PropertyTypeId { get; set; }


        [Required(ErrorMessage = "Location is required!")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Populated place is required!")]
        public int PopulatedPlaceId { get; set; }

        public int? TotalBedRooms { get; init; }

        public int? TotalBathRooms { get; init; }

        public int? TotalGarages { get; init; }

        public IEnumerable<LocationViewModel> Locations { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; set; }

        public string PriceFormat()
        {
            return this.Price.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));
        }
    }
}
