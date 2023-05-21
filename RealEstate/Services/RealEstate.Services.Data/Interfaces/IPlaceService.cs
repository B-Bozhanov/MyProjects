namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IPlaceService
    {
        public IEnumerable<T> Get<T>();

        public IEnumerable<T> GetPlacesByRegionId<T>(int id);
    }
}
