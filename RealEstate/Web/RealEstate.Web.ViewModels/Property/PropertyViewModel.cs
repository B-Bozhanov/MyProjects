namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;

    using RealEstate.Web.ViewModels.ApplicationUser;
    using RealEstate.Web.ViewModels.ImageModel;
    using RealEstate.Web.ViewModels.PopulatedPlaces;

    public class PropertyViewModel : BasePropertyModel
    {
        public UserViewModel ApplicationUser { get; init; }

        public PopulatedPlaceViewModel PopulatedPlace { get; init; }

        public string PropertyTypeName { get; set; }

        public bool IsExpired { get; set; }

        // TODO: Move to GlobalConstants
        public string ExpireMessage { get; init; } = $"Expired!";

        public ICollection<ImageViewModel> Images { get; init; }
    }
}
