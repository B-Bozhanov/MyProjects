namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.Districts;
    using RealEstate.Web.ViewModels.Places;
    using RealEstate.Web.ViewModels.Property;

    public interface IPropertyService
    {
        void Add(AddPropertyModel propertyModel, [CallerMemberName] string import = null!);

        public IEnumerable<Property> GetProperties();

        public IEnumerable<PropertyTypeViewModel> GetPropertiesTypes();

        public IEnumerable<PlaceModel> GetPlaces();

        public IEnumerable<DistrictModel> GetDistricts();

        public IList<BuildingTypeModel> GetBuildingsTypes();

        public IEnumerable<PropertyViewModel> GetTop10NewestSells();
    }
}
