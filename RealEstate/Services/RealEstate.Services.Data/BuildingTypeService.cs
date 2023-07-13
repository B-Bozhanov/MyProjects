namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.BuildingTypeModel;

    public class BuildingTypeService : IBuildingTypeService
    {
        private readonly IDeletableEntityRepository<BuildingType> buildingTypeRepository;

        public BuildingTypeService(IDeletableEntityRepository<BuildingType> buildingTypeRepository)
        {
            this.buildingTypeRepository = buildingTypeRepository;
        }

        public IList<BuildingTypeViewModel> GetAll() 
            => this.buildingTypeRepository
                .All()
                .OrderBy(pt => pt.Name)
                .To<BuildingTypeViewModel>()
                .ToList();
    }
}
