namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    public class DistrictService : IDistrictService
    {
        private readonly IDeletableEntityRepository<District> districtRepository;

        public DistrictService(IDeletableEntityRepository<District> districtRepository)
        {
            this.districtRepository = districtRepository;
        }

        public IEnumerable<T> Get<T>()
            => this.districtRepository
            .All()
            .OrderBy(d => d.Name)
            .To<T>()
            .ToList();

        public ICollection<T> GetDistrictByDownTownId<T>(string id)
            => this.districtRepository
            .All()
            .Where(d => d.DownTownId == id)
            .OrderBy(d => d.Name)
            .To<T>()
            .ToList();

    }
}
