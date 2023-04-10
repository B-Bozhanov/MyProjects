namespace RealEstate.Models.ImportViewModels
{
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Models.DataModels;

    public class PropertyViewModel
    {
        public PropertyViewModel()
        {
            this.ExpirationDays = 90; // By default
            this.Option = PropertyOption.Sale; // By default
            this.Images = new List<Image>();
        }

        public string? Url { get; set; }

        public int Size { get; set; }

        public int ExpirationDays { get; set; }

        public PropertyOption Option { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        [Required]
        public string? District { get; set; }

        public int Year { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? BuildingType { get; set; }

        public decimal? Price { get; set; }

        public ICollection<Image>? Images { get; set; }

        public int TypeId { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypeViewModels { get; set; }
    }
}
