namespace RealEstate.Data.Repositories
{
    using RealEstate.Data.Interfaces;
    using RealEstate.Data.Repositories.Base;
    using RealEstate.Models.DataModels;

    public class BuildingTypeRepository : BaseRepository<BuildingType>, IBuildingTypeRepository
    {
        public BuildingTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
