namespace RealEstate.Models.ImportModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Models.DataModels;

    public class AddPropertyModel
    {
        public AddPropertyModel()
        {
            Images = new List<Image>();
        }

        public decimal? Price { get; set; }

        public string? Url { get; set; }

        public int Size { get; set; }

        [Required]
        [Display(Name = "Ad Expiration")]
        public int ExpirationDays { get; set; }

        [Required]
        [Display(Name = "Options")]
        public PropertyOption Option { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public int? TotalBedRooms { get; set; }

        public int? TotalBathRooms { get; set; }

        public int? TotalGarages { get; set; }

        public string? Condition { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(1800, 2022)]
        public int Year { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required!")]
        [MinLength(3, ErrorMessage = "The min length is 3")]
        [Display(Name = "Place")]
        public string PlaceName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required!")]
        [MinLength(3, ErrorMessage = "The min length is 3")]
        [Display(Name = "District")]
        public string District { get; set; } = null!;

        [Required(AllowEmptyStrings = false,ErrorMessage = "The field is required!")]
        [MinLength(3, ErrorMessage = "The min length is 3")]
        [Display(Name = "Type")]
        public string Type { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "The field is required!")]
        //[MinLength(2, ErrorMessage = "The min length is 3")]
        //[Display(Name = "Building Type")]
        public string BuildingType { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required!")]
        [Display(Name = "Select Building Type")]
        public int BuildingTypeId { get; set; }

        [Required]
        public ContactsModel ContactsModel { get; set; }

        public ICollection<Image>? Images { get; set; } = new List<Image>();

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; init; }

        public IEnumerable<BuildingTypeModel> BuildingTypes { get; set; }

        public IEnumerable<PlacesModel> Places { get; init; }

        public IEnumerable<DistrictsModel> Districts { get; set; }
    }
}
