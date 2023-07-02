namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Property;

    public interface IPropertyService
    {
        public int GetAllCount();

        public Task AddAsync(PropertyInputModel propertyModel, ApplicationUser user, [CallerMemberName] string import = null!);

        public Task Edit(PropertyEditViewModel editModel);

        public Task<IEnumerable<PropertyViewModel>> GetPaginationByUserId(string id, int page);

        public IEnumerable<PropertyViewModel> GetAllByOptionId(int optionId);

        public IEnumerable<PropertyViewModel> GetTopNewest(int count);

        public IEnumerable<PropertyViewModel> GetTopMostExpensive(int count);

        public Task<T> GetByIdAsync<T>(int id);

        public Task<T> GetByIdAsync<T>(int id, string userId);

        public Task<bool> IsUserProperty(int propertyId, string userId);

        public Task<PropertyInputModel> SetCollectionsAsync(PropertyInputModel property);
    }
}
