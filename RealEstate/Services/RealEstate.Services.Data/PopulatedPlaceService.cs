namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    public class PopulatedPlaceService : IPopulatedPlaceService
    {
        private readonly IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository;

        public PopulatedPlaceService(IDeletableEntityRepository<PopulatedPlace> placeRepository)
        {
            this.populatedPlaceRepository = placeRepository;
        }

        public IEnumerable<T> Get<T>()
            => this.populatedPlaceRepository
            .All()
            .OrderBy(p => p.Name)
            .To<T>()
            .ToList();

        public IEnumerable<T> GetPopulatedPlacesByLocationId<T>(int id)
            => this.populatedPlaceRepository
            .All()
            .Where(p => p.LocationId == id)
            .OrderBy(p => p.Name)
            .To<T>()
            .ToList();

        public T GetPopulatedPlacesByProperty<T>(int propertyId)
           => this.populatedPlaceRepository
           .All()
           .Where(p => p.Id == propertyId)
           .To<T>()
           .FirstOrDefault();
    }
}
