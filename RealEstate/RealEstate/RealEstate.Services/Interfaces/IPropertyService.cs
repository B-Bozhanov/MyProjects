namespace RealEstate.Services.Interfaces
{
    using System.Runtime.CompilerServices;

    using RealEstate.Models.DataModels;
    using RealEstate.Models.ImportModels;

    public interface IPropertyService
    {
        void Add(AddPropertyModel propertyModel, [CallerMemberName]string import = null!);

        public IEnumerable<Property> GetProperties();

        public IEnumerable<PropertyType> GetPropertiesTypes();

        public IEnumerable<Place> GetPlaces();

        public IEnumerable<District> GetDistricts();

        public IEnumerable<BuildingType> GetBuildingsTypes();

        public IEnumerable<Property> GetTop10Newest();
    }
}
