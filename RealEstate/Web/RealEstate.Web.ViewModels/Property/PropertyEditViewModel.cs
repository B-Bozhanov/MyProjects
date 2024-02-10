namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.ImageModel;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public class PropertyEditViewModel :BasePropertyModel, IMapFrom<Property>
    {
        public bool IsExpired { get; set; }

        public int? YardSize { get; init; }

        public int? Floor { get; init; }

        [Display(Name = "Total Floors")]
        public int? TotalFloors { get; init; }

        public string Condition { get; init; }

        public int? Year { get; init; }

        public int PropertyTypeId { get; set; }

        public BuildingTypeViewModel BuildingType { get; set; }

        public PopulatedPlaceViewModel PopulatedPlace { get; set; }

        [Required(ErrorMessage = "Location is required!")]
        [DisplayName("Location")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Populated place is required!")]
        [DisplayName("Populated Place")]
        public int PopulatedPlaceId { get; init; }

        public ICollection<ImageViewModel> Images { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; set; }

        public IList<BuildingTypeViewModel> BuildingTypes { get; set; }
    }
}
