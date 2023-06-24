namespace RealEstate.Web.ViewModels.Property
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.Infrastructure.CustomAttributes;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.PropertyTypes;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.Xml.Linq;

    using static RealEstate.Common.GlobalConstants.Account;

    public class EditViewModel : IMapFrom<Property>, IHaveCustomMappings
    {
        public int Id { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "The price can not be negative")]
        public decimal Price { get; init; }

        [Required(ErrorMessage = "The field is required!")]
        public int Size { get; init; }

        [Display(Name = "Ad Expiration")]
        public int ExpirationDays { get; init; }

        [Display(Name = "Options")]
        public PropertyOption Option { get; init; }

        public int YardSize { get; init; }

        public int Floor { get; init; }

        [Display(Name = "Total Floors")]
        public int TotalFloors { get; init; }

        [Display(Name = "Bed Rooms")]
        public int TotalBedRooms { get; init; }

        [Display(Name = "Bath Rooms")]
        public int TotalBathRooms { get; init; }

        [Display(Name = "Garages")]
        public int TotalGarages { get; init; }

        public string Condition { get; init; }

        public string Description { get; init; }

        public int? Year { get; init; }

        public int? PropertyTypeId { get; set; } = null!;

        public BuildingTypeViewModel BuildingType { get; set; }

        public PopulatedPlaceViewModel PopulatedPlace { get; set; }

        [Required(ErrorMessage = "Location is required!")]
        [DisplayName("Location")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Populated place is required!")]
        [DisplayName("Populated Place")]
        public int PopulatedPlaceId { get; init; }

        //[Required]
        //[Display(Name = "Contacts")]
        //public ContactModel ContactModel { get; set; } = null!;

        public ICollection<ImageViewModel> Images { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; set; }

        public IList<BuildingTypeViewModel> BuildingTypes { get; set; }

        public IEnumerable<LocationViewModel> Locations { get; set; }

        public IEnumerable<PopulatedPlaceViewModel> PopulatedPlaces { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Property, EditViewModel>()
                 .ForMember(
                dest => dest.Id,
                option => option.MapFrom(x => x.Id)
                );
        }
    }
}
