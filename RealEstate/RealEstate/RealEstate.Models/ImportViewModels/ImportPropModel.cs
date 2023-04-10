namespace RealEstate.Models.ImportViewModels
{
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Models.DataModels;

    public class ImportPropModel
    {
        public ImportPropModel()
        {
            this.ExpirationDays = 90; // By default
            this.Option = PropertyOption.Sale; // By default
            this.Images = new List<byte[]>();
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

        public List<byte[]>? Images { get; set; }
    }
}
