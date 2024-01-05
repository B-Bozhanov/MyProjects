namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;
    using System.Globalization;

    using AutoMapper;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.PopulatedPlaces;

    public class PropertyViewModel : IMapFrom<Property>
    {
        public int Id { get; init; }

        public ApplicationUser ApplicationUser { get; init; }

        public string Description { get; init; }

        public PopulatedPlaceViewModel PopulatedPlace { get; init; }

        public string Option { get; set; }

        public string PropertyTypeName { get; set; }

        public string Price { get; init; }

        public int Size { get; init; }

        public int? TotalBedRooms { get; init; }

        public int? TotalBathRooms { get; init; }

        public int? TotalGarages { get; init; }

        public int ExpirationDays { get; init; }

        // TODO: Math.Round or something for the days.
        public bool IsExpired { get; set; }

        public bool IsExpirationDaysModified { get; set; }

        public int ExpireAfter { get; private set; }

        // TODO: Move to GlobalConstants
        public string ExpireMessage { get; init; } = $"Expired!";

        public ICollection<ImageViewModel> Images { get; init; }
    }
}
