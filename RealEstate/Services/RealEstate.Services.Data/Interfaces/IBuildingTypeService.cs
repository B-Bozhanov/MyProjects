namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using RealEstate.Web.ViewModels.BuildingTypeModel;

    public interface IBuildingTypeService
    {
        public IList<BuildingTypeViewModel> GetAll();
    }
}
