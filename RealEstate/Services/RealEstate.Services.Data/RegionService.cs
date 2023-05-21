namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    public class RegionService : IRegionService
    {
        private readonly IDeletableEntityRepository<Location> regionRepository;

        public RegionService(IDeletableEntityRepository<Location> regionRepository)
        {
            this.regionRepository = regionRepository;
        }

        public IEnumerable<T> Get<T>()
            => this.regionRepository
                .All()
                .OrderBy(p => p.Name)
                .To<T>()
                .ToList();
    }
}
