namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    public class DownTownService : IDownTownService
    {
        private readonly IDeletableEntityRepository<DownTown> downTownRepository;

        public DownTownService(IDeletableEntityRepository<DownTown> downTownRepository)
        {
            this.downTownRepository = downTownRepository;
        }

        public IEnumerable<T> Get<T>()
            => this.downTownRepository
            .All()
            .OrderBy(d => d.Name)
            .To<T>()
            .ToList();
    }
}
