namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    public class PlaceService : IPlaceService
    {
        private readonly IDeletableEntityRepository<PopulatedPlace> placeRepository;

        public PlaceService(IDeletableEntityRepository<PopulatedPlace> placeRepository)
        {
            this.placeRepository = placeRepository;
        }

        public IEnumerable<T> Get<T>()
            => this.placeRepository
            .All()
            .OrderBy(p => p.Name)
            .To<T>()
            .ToList();

        public IEnumerable<T> GetPlacesByRegionId<T>(int id)
            => this.placeRepository
            .All()
            .Where(p => p.LocationId == id)
            .OrderBy(p => p.Name)
            .To<T>()
            .ToList();
    }
}
