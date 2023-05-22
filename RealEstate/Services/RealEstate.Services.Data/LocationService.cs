namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    public class LocationService : ILocationService
    {
        private readonly IDeletableEntityRepository<Location> locationService;

        public LocationService(IDeletableEntityRepository<Location> regionRepository)
        {
            this.locationService = regionRepository;
        }

        public IEnumerable<T> Get<T>()
            => this.locationService
                .All()
                .OrderBy(p => p.Name)
                .To<T>()
                .ToList();

        public void SaveToFile(string file)
        {
            File.WriteAllText("..\\..\\Data\\RealEstate.Data\\Seeding\\DataToSeed\\locations.json", file);
        }
    }
}
