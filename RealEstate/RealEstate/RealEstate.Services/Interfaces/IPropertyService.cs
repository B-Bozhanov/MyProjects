namespace RealEstate.Services.Interfaces
{
    using System.Runtime.CompilerServices;

    using RealEstate.Models.DataModels;
    using RealEstate.Models.ImportModels;

    public interface IPropertyService
    {
        void Add(AddPropertyModel propertyModel, [CallerMemberName]string import = null!);

        public IEnumerable<Property> GetProperties();

        public IEnumerable<PropertyTypeViewModel> GetPropertiesTypes();

        public IEnumerable<PlacesModel> GetPlaces();

        public IEnumerable<DistrictsModel> GetDistricts();

        public IList<BuildingTypeModel> GetBuildingsTypes();

        public IEnumerable<Property> GetTop10Newest();
    }
}
