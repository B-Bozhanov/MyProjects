namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Web.ViewModels.DetailModel;

    public interface IDetailService
    {
        public Task<IList<DetailViewModel>> GetAllAsync();
    }
}
