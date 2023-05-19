namespace RealEstate.Services.Data
{
    using System.Collections.Generic;

    using RealEstate.Services.Data.Interfaces;

    // TODO: BaseService abstract class
    public class BaseService : IService
    {
        public IEnumerable<T> Get<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}
