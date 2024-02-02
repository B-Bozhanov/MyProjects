namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;
    using System.Globalization;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.ImageModel;
    using RealEstate.Web.ViewModels.PopulatedPlaces;

    public class PropertyViewModel : BasePropertyModel
    {
        public ApplicationUser ApplicationUser { get; init; }

        public PopulatedPlaceViewModel PopulatedPlace { get; init; }

        public string PropertyTypeName { get; set; }

        public bool IsExpired { get; set; }

        public bool IsExpirationDaysModified { get; set; }

        // TODO: Move to GlobalConstants
        public string ExpireMessage { get; init; } = $"Expired!";

        public ICollection<ImageViewModel> Images { get; init; }
    }
}
