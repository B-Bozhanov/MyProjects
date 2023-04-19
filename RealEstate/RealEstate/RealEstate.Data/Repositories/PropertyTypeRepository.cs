namespace RealEstate.Data.Repositories
{
    using RealEstate.Data.Interfaces;
    using RealEstate.Data.Repositories.Base;
    using RealEstate.Models.DataModels;

    public class PropertyTypeRepository : BaseRepository<PropertyType>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
