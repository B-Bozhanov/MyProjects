namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IPopulatedPlaceService
    {
        public IEnumerable<T> Get<T>();

        public IEnumerable<T> GetPopulatedPlacesByLocationId<T>(int id);

        public T GetPopulatedPlacesByProperty<T>(int propertyId);
    }
}
