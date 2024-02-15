namespace RealEstate.Web.ViewModels.Property
{
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PropertyTypes;
    using System.Collections.Generic;

    public interface IBasePropertyModel
    {
        public int Id { get; init; }

        public int PropertyTypeId { get; set; }

        public int LocationId { get; set; }

        public int PopulatedPlaceId { get; set; }

        public IEnumerable<LocationViewModel> Locations { get; set; }

        public IEnumerable<PropertyTypeViewModel> PropertyTypes { get; set; }
    }
}
