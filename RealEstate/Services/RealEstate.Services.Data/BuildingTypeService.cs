namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    public class BuildingTypeService : IBuildingTypeService
    {
        private readonly IDeletableEntityRepository<BuildingType> buildingTypeRepository;

        public BuildingTypeService(IDeletableEntityRepository<BuildingType> buildingTypeRepository)
        {
            this.buildingTypeRepository = buildingTypeRepository;
        }

        public IList<T> Get<T>()
        {
            return this.buildingTypeRepository
                .All()
                .OrderBy(pt => pt.Name)
                .To<T>()
                .ToList();
        }
    }
}
