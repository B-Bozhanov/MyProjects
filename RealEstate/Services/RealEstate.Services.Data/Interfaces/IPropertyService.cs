namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.Search;

    public interface IPropertyService
    {
        public Task AddAsync(PropertyInputModel propertyModel, string userId);

        public Task EditAsync(PropertyEditViewModel editModel);

        public Task<bool> IsAnyExpiredProperties(string userId);

        public Task<bool> IsUserPropertyAsync(int propertyId, string userId);

        public Dictionary<string, List<string>> PropertyValidator(PropertyInputModel property);

        public Task<bool> RemoveByIdAsync(int id);

        public Task<IEnumerable<PropertyViewModel>> SearchAsync(SearchInputModel searchModel);
    }
}
