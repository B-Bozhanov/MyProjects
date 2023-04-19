namespace RealEstate.Data.Repositories
{
    using RealEstate.Data.Interfaces;
    using RealEstate.Data.Repositories.Base;
    using RealEstate.Models.DataModels;

    public class DistrictRepository : BaseRepository<District>, IDistrictRepository
    {
        public DistrictRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
