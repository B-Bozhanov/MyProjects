namespace RealEstate.Web.ViewModels.Property
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public abstract class BasePropertyModel : IMapFrom<Property>
    {
        public int Id { get; init; }

        public string Description { get; init; }

        public int ExpirationDays { get; set; }

        public PropertyOption Option { get; set; }

        public decimal Price { get; init; }

        public int Size { get; init; }

        public int? TotalBedRooms { get; init; }

        public int? TotalBathRooms { get; init; }

        public int? TotalGarages { get; init; }
    }
}
