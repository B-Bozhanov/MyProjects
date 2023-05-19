namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IDistrictService
    {
        public IEnumerable<T> Get<T>();

        public IEnumerable<T> GetDistrictByDownTownId<T>(string id);
    }
}
