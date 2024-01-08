namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Hangfire.Server;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.Search;

    public interface IPropertyService
    {
        public int GetAllActiveCount();

        public Task AddAsync(PropertyInputModel propertyModel, ApplicationUser user, [CallerMemberName] string import = null!);

        public Task EditAsync(PropertyEditViewModel editModel);

        public Task<IEnumerable<PropertyViewModel>> GetActiveUserPropertiesPerPageAsync(string id, int page);

        public Task<IEnumerable<PropertyViewModel>> GetExpiredUserPropertiesPerPageAsync(string id, int page);

        public Task<IEnumerable<PropertyViewModel>> GetAllWithExpiredUserPropertiesPerPage(string id, int page);

        public Task<bool> IsAnyExpiredProperties(string userId);

        public Task<int> GetAllExpiredPropertiesCount();

        public int GetAllActiveUserPropertiesCount(string userId);

        public int GetAllExpiredUserPropertiesCount(string userId);

        public IEnumerable<PropertyViewModel> GetAllByOptionIdPerPage(int optionId, int page);

        public IEnumerable<PropertyViewModel> GetTopNewest(int count);

        public IEnumerable<PropertyViewModel> GetTopMostExpensive(int count);

        public Task<T> GetByIdAsync<T>(int id);

        public Task<T> GetByIdWithExpiredAsync<T>(int id, string userId);

        public Task<bool> IsUserProperty(int propertyId, string userId);

        public Dictionary<string, List<string>> PropertyValidator(PropertyInputModel property);

        public Task<bool> RemoveByIdAsync(int id);

        public Task<IEnumerable<PropertyViewModel>> SearchAsync(SearchViewModel searchModel);
    }
}
