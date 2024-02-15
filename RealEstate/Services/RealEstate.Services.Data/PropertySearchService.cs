namespace RealEstate.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.Search;

    public class PropertySearchService : IPropertySearchService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository;

        public PropertySearchService(IDeletableEntityRepository<Property> propertyRepository, IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository)
        {
            this.propertyRepository = propertyRepository;
            this.populatedPlaceRepository = populatedPlaceRepository;
        }

        public async Task<IEnumerable<T>> SearchAsync<T>(SearchInputModel searchModel)
        {
            if (searchModel.KeyWord == null && searchModel.Type == null
                && searchModel.BathRooms == null && searchModel.BedRooms == null
                && searchModel.LocationId == default && searchModel.PopulatedPlaceId == default
                && searchModel.Garages == null && searchModel.MinPrice == null && searchModel.MaxPrice == null)
            {
                throw new InvalidOperationException("Select at least one criteria");
            }
            if (searchModel.KeyWord == null && searchModel.Type == null
                && searchModel.BathRooms == null && searchModel.BedRooms == null
                && searchModel.LocationId == default && searchModel.PopulatedPlaceId == default
                && searchModel.Garages == null && searchModel.MinPrice == null && searchModel.MaxPrice != null)
            {
                return await this.SearchByMaxPriceAsync<T>(searchModel.MaxPrice);
            }
            if (searchModel.KeyWord == null && searchModel.Type == null
               && searchModel.BathRooms == null && searchModel.BedRooms == null
               && searchModel.LocationId == default && searchModel.PopulatedPlaceId == default
               && searchModel.Garages == null && searchModel.MinPrice != null && searchModel.MaxPrice == null)
            {
                return await this.SearchByMinPriceAsync<T>(searchModel.MinPrice);
            }
            if (searchModel.KeyWord == null && searchModel.Type == null
              && searchModel.BathRooms == null && searchModel.BedRooms == null
              && searchModel.LocationId == default && searchModel.PopulatedPlaceId == default
              && searchModel.Garages == null && searchModel.MinPrice != null && searchModel.MaxPrice != null)
            {
                if (searchModel.MinPrice > searchModel.MaxPrice)
                {
                    throw new InvalidOperationException("The Mininum price can not be more than Maxumum price");
                }

                return await this.SearchBetweenMinAndMaxPriceAsync<T>(searchModel.MinPrice, searchModel.MaxPrice);
            }
            if (searchModel.KeyWord == null && searchModel.Type != null
             && searchModel.BathRooms == null && searchModel.BedRooms == null
             && searchModel.LocationId == default && searchModel.PopulatedPlaceId == default
             && searchModel.Garages == null && searchModel.MinPrice == null && searchModel.MaxPrice == null)
            {
                return await this.SearchByTypeAsync<T>(searchModel.Type);
            }
            if (searchModel.KeyWord == null && searchModel.Type != null
            && searchModel.BathRooms == null && searchModel.BedRooms == null
            && searchModel.LocationId == default && searchModel.PopulatedPlaceId == default
            && searchModel.Garages == null && searchModel.MinPrice != null && searchModel.MaxPrice == null)
            {
                return await this.SearchByTypeAndMinPriceAsync<T>(searchModel.Type, (int)searchModel.MinPrice);
            }
            if (searchModel.KeyWord == null && searchModel.Type != null
            && searchModel.BathRooms == null && searchModel.BedRooms == null
            && searchModel.LocationId == default && searchModel.PopulatedPlaceId == default
            && searchModel.Garages == null && searchModel.MinPrice == null && searchModel.MaxPrice != null)
            {
                return await this.SearchByTypeAndMaxPriceAsync<T>(searchModel.Type, searchModel.MaxPrice);
            }
            if (searchModel.KeyWord == null && searchModel.Type != null
            && searchModel.BathRooms == null && searchModel.BedRooms == null
            && searchModel.LocationId == default && searchModel.PopulatedPlaceId == default
            && searchModel.Garages == null && searchModel.MinPrice != null && searchModel.MaxPrice != null)
            {
                return await this.SearchByTypeBetweenMinAndMaxPriceAsync<T>(searchModel.Type, searchModel.MinPrice, searchModel.MaxPrice);
            }
            if (searchModel.KeyWord == null && searchModel.Type == null
            && searchModel.BathRooms == null && searchModel.BedRooms == null
            && searchModel.LocationId != default && searchModel.PopulatedPlaceId != default
            && searchModel.Garages == null && searchModel.MinPrice == null && searchModel.MaxPrice == null)
            {
                var populatedPlace = this.populatedPlaceRepository.All().FirstOrDefault(p => p.Id == searchModel.PopulatedPlaceId);

                if (populatedPlace.Name == "Всички")
                {
                    return await this.SearchByLocationIdAsync<T>((int)searchModel.LocationId);
                }
                else
                {
                    return await this.SearchByLocationIdAndPopulatedPlaceIdAsync<T>((int)searchModel.LocationId, (int)searchModel.PopulatedPlaceId);
                }
            }
            return new List<T>();
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
