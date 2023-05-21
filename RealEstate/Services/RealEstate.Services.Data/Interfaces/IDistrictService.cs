namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IDistrictService
    {
        public IEnumerable<T> Get<T>();

        public ICollection<T> GetDistrictByDownTownId<T>(string id);
    }
}
