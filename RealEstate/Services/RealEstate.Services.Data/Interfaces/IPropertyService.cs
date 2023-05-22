namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.Property;

    public interface IPropertyService
    {
        Task Add(AddPropertyViewModel propertyModel, [CallerMemberName] string import = null!);

        public IEnumerable<Property> Get();

        public IEnumerable<LocationViewModel> GetLocations();

        public IEnumerable<PopulatedPlaceViewModel> GetPopulatedPlaces();
    }
}
