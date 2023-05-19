namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IService
    {
        public IEnumerable<T> Get<T>();
    }
}
