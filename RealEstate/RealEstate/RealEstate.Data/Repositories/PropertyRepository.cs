namespace RealEstate.Data.Repositories
{
    using RealEstate.Data.Interfaces;
    using RealEstate.Data.Repositories.Base;
    using RealEstate.Models.DataModels;

    public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
