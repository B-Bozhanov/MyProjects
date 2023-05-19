namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IDownTownService
    {
        public IEnumerable<T> Get<T>();
    }
}
