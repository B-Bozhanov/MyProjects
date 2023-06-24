namespace RealEstate.Web.ViewModels.Property
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.Infrastructure.CustomAttributes;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.ConditionModel;
    using RealEstate.Web.ViewModels.ContactModel;
    using RealEstate.Web.ViewModels.DetailModel;
    using RealEstate.Web.ViewModels.EquipmentModel;
    using RealEstate.Web.ViewModels.HeatingModel;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public class AddPropertyInputModel : IMapFrom<Property>
    {
        [Range(0, double.MaxValue, ErrorMessage = "The price can not be negative")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The field is required!")]
        public int Size { get; set; }

        [Display(Name = "Ad Expiration")]
        public int ExpirationDays { get; set; }

        [Display(Name = "Options")]
        public PropertyOption Option { get; set; }

        public int YardSize { get; set; }

        public int Floor { get; set; }

        [Display(Name = "Total Floors")]
        public int TotalFloors { get; set; }

        [Display(Name = "Bed Rooms")]
        public int TotalBedRooms { get; set; }

        [Display(Name = "Bath Rooms")]
        public int TotalBathRooms { get; set; }

        [Display(Name = "Garages")]
        public int TotalGarages { get; set; }

        public string Condition { get; set; }

        public string Description { get; set; }

        [YearValidator(ErrorMessage = "Test")]
        public int Year { get; set; }

        public int? TypeId { get; set; } = null!;

        [Required(ErrorMessage = "Location is required!")]
        [DisplayName("Location")]
        public string LocationId { get; set; }

        [Required(ErrorMessage = "Populated place is required!")]
        [DisplayName("Populated Place")]
        public int PopulatedPlaceId { get; set; }

        [Required]
        [Display(Name = "Contacts")]
        public ContactModel ContactModel { get; set; } = null!;

        [FileExtensions(Extensions = "jpeg, jpg")]
        public IFormFileCollection Images { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; set; }

        public IEnumerable<LocationViewModel> Locations { get; set; }

        public IList<BuildingTypeViewModel> BuildingTypes { get; set; }

        public IList<ConditionViewModel> Conditions { get; set; }

        public IList<EquipmentViewModel> Equipments { get; set; }

        public IList<DetailViewModel> Details { get; set; }

        public IList<HeatingViewModel> Heatings { get; set; }
    }
}
