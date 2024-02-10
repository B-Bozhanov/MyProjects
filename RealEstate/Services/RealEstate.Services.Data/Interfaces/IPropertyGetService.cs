namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;

    public interface IPropertyGetService
    {
        public Task<IEnumerable<T>> GetActiveUserPropertiesPerPageAsync<T>(string id, int page);

        public Task<Property> GetByIdAsync(int id);

        public int GetAllActiveCount();

        public int GetAllActiveUserPropertiesCount(string userId);

        public int GetAllExpiredPropertiesCount();

        public int GetAllExpiredUserPropertiesCount(string userId);

        public IEnumerable<T> GetAllByOptionIdPerPage<T>(int optionId, int page);

        public Task<IEnumerable<T>> GetAllWithExpiredUserPropertiesPerPage<T>(string id, int page);

        public Task<T> GetByIdAsync<T>(int id);

        public Task<T> GetByIdWithExpiredUserPropertiesAsync<T>(int id, string userId);

        public Task<IEnumerable<T>> GetExpiredUserPropertiesPerPageAsync<T>(string id, int page);

        public IEnumerable<T> GetTopNewest<T>(int count);

        public IEnumerable<T> GetTopMostExpensive<T>(int count);
    }
}
