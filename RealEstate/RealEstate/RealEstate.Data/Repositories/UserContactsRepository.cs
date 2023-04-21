namespace RealEstate.Data.Repositories
{
    using RealEstate.Data.Interfaces;
    using RealEstate.Data.Repositories.Base;
    using RealEstate.Models.DataModels;

    public class UserContactsRepository : BaseRepository<UserContact>, IUserContactsRepository
    {
        public UserContactsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
