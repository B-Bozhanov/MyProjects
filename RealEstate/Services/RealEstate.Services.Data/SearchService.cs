namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Property;

    public class SearchService : ISearchService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;

        public SearchService(IDeletableEntityRepository<Property> propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public async Task<IEnumerable<T>> SearchBetweenMinAndMaxPriceAsync<T>(decimal? minPrice, decimal? maxPrice)
                => await this.propertyRepository.All()
                        .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByLocationIdAndMaxPriceAsync<T>(int locationId, decimal? maxPrice)
                 => await this.propertyRepository.All()
                        .Where(p => p.LocationId == locationId && p.Price <= maxPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByLocationIdAndMinPriceAsync<T>(int locationId, decimal? minPrice)
                => await this.propertyRepository.All()
                        .Where(p => p.LocationId == locationId && p.Price >= minPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByLocationIdAndPopulatedPlaceIdAsync<T>(int locationId, int populatedPlaceId)
                => await this.propertyRepository.All()
                        .Where(p => p.LocationId == locationId && p.PopulatedPlaceId == populatedPlaceId)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByLocationIdAsync<T>(int locationId)
                => await this.propertyRepository.All()
                        .Where(p => p.LocationId == locationId)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByLocationIdBetweenMinAndMaxPriceAsync<T>(int locationId, decimal? minPrice, decimal? maxPrice)
                => await this.propertyRepository.All()
                        .Where(p => p.LocationId == locationId && p.Price >= minPrice && p.Price <= maxPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByMaxPriceAsync<T>(decimal? maxPrice)
                => await this.propertyRepository.All()
                        .Where(p => p.Price <= maxPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByMinPriceAsync<T>(decimal? minPrice)
                => await this.propertyRepository.All()
                        .Where(p => p.Price >= minPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByTypeAndMaxPriceAsync<T>(PropertyOptionModel? type, decimal? maxPrice)
                => await this.propertyRepository.All()
                        .Where(p => p.Option== (PropertyOption)type && p.Price <= maxPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByTypeAndMinPriceAsync<T>(PropertyOptionModel? type, decimal? minPrice)
                 => await this.propertyRepository.All()
                        .Where(p => p.Option== (PropertyOption)type && p.Price >= minPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByTypeAsync<T>(PropertyOptionModel? type)
                => await this.propertyRepository.All()
                        .Where(p => p.Option == (PropertyOption)type)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();

        public async Task<IEnumerable<T>> SearchByTypeBetweenMinAndMaxPriceAsync<T>(PropertyOptionModel? type, decimal? minPrice, decimal? maxPrice)
                => await this.propertyRepository.All()
                        .Where(p => p.Option == (PropertyOption)type && p.Price >= minPrice && p.Price <= maxPrice)
                        .OrderBy(p => p.Price)
                        .To<T>()
                        .ToListAsync();
    }
}
