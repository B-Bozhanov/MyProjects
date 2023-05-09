namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.ContactModel;
    using RealEstate.Web.ViewModels.Districts;
    using RealEstate.Web.ViewModels.Places;

    public class AddPropertyModel
    {
        [Range(0, double.MaxValue, ErrorMessage = "The price can not be negative")]
        public decimal? Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required!")]
        public int Size { get; set; }

        [Display(Name = "Ad Expiration")]
        public int ExpirationDays { get; set; }

        [Required]
        [Display(Name = "Options")]
        public PropertyOption Option { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        [Display(Name = "Total Floors")]
        public int? TotalFloors { get; set; }

        [Display(Name = "Bed Rooms")]
        public int? TotalBedRooms { get; set; }

        [Display(Name = "Bath Rooms")]
        public int? TotalBathRooms { get; set; }

        [Display(Name = "Garages")]
        public int? TotalGarages { get; set; }

        public string Condition { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1800, 2022)]
        public int Year { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required!")]
        [MinLength(3, ErrorMessage = "The min length is 3")]
        [Display(Name = "Place")]
        public string PlaceName { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required!")]
        [MinLength(3, ErrorMessage = "The min length is 3")]
        [Display(Name = "District")]
        public string District { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required!")]
        [MinLength(3, ErrorMessage = "The min length is 3")]
        [Display(Name = "Type")]
        public string Type { get; set; } = null!;

        public int? TypeId { get; set; } = null!;

        [MinLength(2, ErrorMessage = "The min length is 3")]
        [Display(Name = "Building Type")]
        public string BuildingType { get; set; }

        [Required]
        [Display(Name = "Contacts")]
        public ContactModel ContactModel { get; set; } = null!;

        [FileExtensions(Extensions = "jpeg, jpg")]
        public IFormFileCollection Images { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; init; }

        public IList<BuildingTypeModel> BuildingTypes { get; set; }

        public IEnumerable<PlaceModel> Places { get; init; }

        public IEnumerable<DistrictModel> Districts { get; set; }
    }
}
