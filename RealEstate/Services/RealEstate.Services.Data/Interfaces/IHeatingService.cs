namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Web.ViewModels.HeatingModel;

    public interface IHeatingService
    {
        public Task<IList<HeatingViewModel>> GetAllAsync();
    }
}
