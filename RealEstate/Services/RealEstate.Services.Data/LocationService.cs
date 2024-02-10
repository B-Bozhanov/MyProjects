namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;

    public class LocationService : ILocationService
    {
        private readonly IDeletableEntityRepository<Location> locationRepository;

        public LocationService(IDeletableEntityRepository<Location> locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public IEnumerable<T> Get<T>()
            => this.locationRepository
                .All()
                .OrderBy(p => p.Name)
                .To<T>()
                .ToList();

        public IEnumerable<LocationViewModel> GetLocations()
        {
            return this.locationRepository.All().Select( x => new LocationViewModel
            {
                Id = x.Id,
                Name = x.Name,
                PopulatedPlaces = x.PopulatedPlaces.Select(p => new PopulatedPlaceViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList(),
            });
        }

        public void SaveToFile(string file)
        {
            File.WriteAllText("..\\..\\Data\\RealEstate.Data\\Seeding\\DataToSeed\\locations.json", file);
        }
    }
}
