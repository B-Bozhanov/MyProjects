namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Web.ViewModels.EquipmentModel;

    public interface IEquipmentService
    {
        public Task<IList<EquipmentViewModel>> GetAllAsync();
    }
}
