namespace RealEstate.Models.ImportModels
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Models.DataModels;

    public class AddPropertyModel
    {
        public decimal? Price { get; set; }
        public AddPropertyModel()
        {
            ExpirationDays = 90; // By default
            Option = PropertyOption.Sale; // By default
            Images = new List<Image>();
        }

        public string? Url { get; set; }

        public int Size { get; set; }

        public int ExpirationDays { get; set; }

        [Display(Name = "Options")]
        public PropertyOption Option { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public string? Condition { get; set; }

        public string? Description { get; set; }

        public int Year { get; set; }

        [Required]
        [Display(Name = "Place")]
        public string PlaceName { get; set; }

        [Required]
        [Display(Name = "District")]
        public string DistrictName { get; set; } = null!;

        [Required]
        [Display(Name = "Type")]
        public string PropertyTypeName { get; set; } = null!;

        [Required]
        [Display(Name = "Building Type")]
        public string BuildingTypeName { get; set; } = null!;

        public ICollection<Image>? Images { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; init; }

        [BindProperty]
        public List<BuildingTypeModel> BuildingTypes { get; set; } = new List<BuildingTypeModel>();

        public IEnumerable<PlacesModel> Places { get; init; }

        public IEnumerable<DistrictsModel> Districts { get; set; }
    }
}
