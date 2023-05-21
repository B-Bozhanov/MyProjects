namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IBuildingTypeService
    {
        public IList<T> Get<T>();
    }
}
