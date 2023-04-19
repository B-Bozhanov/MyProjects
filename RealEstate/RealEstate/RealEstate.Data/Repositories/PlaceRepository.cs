namespace RealEstate.Data.Repositories
{
    using RealEstate.Data.Interfaces;
    using RealEstate.Data.Repositories.Base;
    using RealEstate.Models.DataModels;

    public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
    {
        public PlaceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
