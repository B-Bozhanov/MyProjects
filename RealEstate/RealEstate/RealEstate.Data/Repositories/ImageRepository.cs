namespace RealEstate.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Data.Interfaces;
    using RealEstate.Data.Repositories.Base;
    using RealEstate.Models.DataModels;

    public class ImageRepository : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public ICollection<Image> GetByPropertId(int id) => this.context.Images.Where(i => i.PropertyId == id).ToList();
    }
}
