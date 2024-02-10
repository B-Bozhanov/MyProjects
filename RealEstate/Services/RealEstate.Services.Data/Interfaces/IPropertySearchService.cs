namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.Search;

    public interface IPropertySearchService
    {
        public Task<IEnumerable<T>> SearchAsync<T>(SearchInputModel searchInputModel);

        public Task<IEnumerable<T>> SearchByMinPriceAsync<T>(decimal? minPrice);

        public Task<IEnumerable<T>> SearchByMaxPriceAsync<T>(decimal? maxPrice);

        public Task<IEnumerable<T>> SearchBetweenMinAndMaxPriceAsync<T>(decimal? minPrice, decimal? maxPrice);

        public Task<IEnumerable<T>> SearchByTypeAsync<T>(PropertyOptionModel? type);

        public Task<IEnumerable<T>> SearchByTypeAndMaxPriceAsync<T>(PropertyOptionModel? type, decimal? maxPrice);

        public Task<IEnumerable<T>> SearchByTypeAndMinPriceAsync<T>(PropertyOptionModel? type, decimal? minPrice);

        public Task<IEnumerable<T>> SearchByTypeBetweenMinAndMaxPriceAsync<T>(PropertyOptionModel? type, decimal? minPrice, decimal? maxPrice);

        public Task<IEnumerable<T>> SearchByLocationIdAsync<T>(int locationId);

        public Task<IEnumerable<T>> SearchByLocationIdAndPopulatedPlaceIdAsync<T>(int locationId, int populatedPlaceId);

        public Task<IEnumerable<T>> SearchByLocationIdBetweenMinAndMaxPriceAsync<T>(int locationId, decimal? minPrice, decimal? maxPrice);

        public Task<IEnumerable<T>> SearchByLocationIdAndMaxPriceAsync<T>(int locationId, decimal? maxPrice);

        public Task<IEnumerable<T>> SearchByLocationIdAndMinPriceAsync<T>(int locationId, decimal? minPrice);
    }
}
