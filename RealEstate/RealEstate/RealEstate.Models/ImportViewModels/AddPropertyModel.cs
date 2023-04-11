namespace RealEstate.Models.ImportViewModels
{
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Models.DataModels;

    using System.ComponentModel.DataAnnotations;

    public class AddPropertyModel
    {
        public decimal? Price { get; set; }
        public AddPropertyModel()
        {
            this.ExpirationDays = 90; // By default
            this.Option = PropertyOption.Sale; // By default
            this.Images = new List<Image>();
        }

        public string? Url { get; set; }

        public int Size { get; set; }

        public int ExpirationDays { get; set; }

        [Display(Name ="Options")]
        public PropertyOption Option { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public string? Condition { get; set; }

        public string? Description { get; set; }

        [Required]
        public string? District { get; set; }

        public int Year { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? BuildingType { get; set; }

        public ICollection<Image>? Images { get; set; }

        [Display(Name = "Type")]
        public int PropertyTypeId { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; init; }

        [Display(Name = "Building Type")]
        public int BuildingTypeId { get; set; }

        [BindProperty]
        public List<BuildingTypeViewModel> BuildingTypes { get; set; }

        [Display(Name = "Place")]
        public int PlaceId { get; set; }

        public IEnumerable<PlacesViewModel> Places { get; init; }

        [Display(Name = "District")]
        public int DistrictId { get; set; }

        public IEnumerable<DistrictsViewModel> Districts { get; set; }
    }
}
