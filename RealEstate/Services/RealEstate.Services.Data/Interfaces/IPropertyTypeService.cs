namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IPropertyTypeService 
    {
        public IEnumerable<T> Get<T>();
    }
}
