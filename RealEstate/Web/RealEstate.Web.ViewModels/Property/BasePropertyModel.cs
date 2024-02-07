namespace RealEstate.Web.ViewModels.Property
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public abstract class BasePropertyModel : IMapFrom<Property>
    {
        public int Id { get; init; }

        [Required]
        public string Description { get; init; }

        public int ExpirationDays { get; set; }

        public PropertyOptionModel Option { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The price can not be negative")]
        public decimal Price { get; init; }

        [Required(ErrorMessage = "The field is required!")]
        public int Size { get; init; }

        public int? TotalBedRooms { get; init; }

        public int? TotalBathRooms { get; init; }

        public int? TotalGarages { get; init; }

        public string PriceFormat()
        {
            return this.Price.ToString("C", CultureInfo.GetCultureInfo("fr-FR"));
        }
    }
}
