namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Districts;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.Regions;

    public interface IPropertyService
    {
        void Add(AddPropertyModel propertyModel, [CallerMemberName] string import = null!);

        public IEnumerable<Property> Get();

        public IEnumerable<RegionViewModel> GetRegions();

        public IEnumerable<DistrictModel> GetDistricts();

        public IEnumerable<AddPropertyViewModel> GetTop10NewestSells();
    }
}
