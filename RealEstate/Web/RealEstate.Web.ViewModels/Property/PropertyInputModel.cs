namespace RealEstate.Web.ViewModels.Property
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    // using RealEstate.Web.Infrastructure.CustomAttributes;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.ConditionModel;
    using RealEstate.Web.ViewModels.ContactModel;
    using RealEstate.Web.ViewModels.DetailModel;
    using RealEstate.Web.ViewModels.EquipmentModel;
    using RealEstate.Web.ViewModels.HeatingModel;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public class PropertyInputModel : BasePropertyModel
    {
        public int? YardSize { get; set; }

        public int Floor { get; set; }

        public int TotalFloors { get; set; }

        // [YearValidator(ErrorMessage = "Test")]
        public int? Year { get; set; }

        [Required]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "Location is required!")]
        public string LocationId { get; set; }

        [Required(ErrorMessage = "Populated place is required!")]
        public int PopulatedPlaceId { get; set; }

        [Required]
        public ContactModel ContactModel { get; set; } = null!;

        //[FileExtensions(Extensions = "jpeg, jpg")]
        [Required(ErrorMessage = "The images is required, min images - 1, max - 20")]
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
