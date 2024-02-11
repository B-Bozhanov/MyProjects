namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;

    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.ImageModel;

    public class PropertyEditViewModel :BasePropertyModel
    {
        public bool IsExpired { get; set; }

        public BuildingTypeViewModel BuildingType { get; set; }

        public ICollection<ImageViewModel> Images { get; set; }

        public IList<BuildingTypeViewModel> BuildingTypes { get; set; }
    }
}
