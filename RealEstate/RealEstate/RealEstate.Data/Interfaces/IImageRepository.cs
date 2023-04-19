namespace RealEstate.Data.Interfaces
{
    using RealEstate.Models.DataModels;

    public interface IImageRepository : IBaseRepository<Image>
    {
        public ICollection<Image> GetByPropertId(int id);
    }
}
