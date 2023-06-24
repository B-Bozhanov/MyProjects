namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Web.ViewModels.ConditionModel;

    public interface IConditionService
    {
        public Task<IList<ConditionViewModel>> GetAllAsync();
    }
}
