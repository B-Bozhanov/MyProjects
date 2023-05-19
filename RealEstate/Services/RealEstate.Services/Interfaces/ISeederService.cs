namespace RealEstate.Services.Interfaces
{
    using System.Collections.Generic;

    public interface ISeederService
    {
        public IEnumerable<T> GetData<T>();
    }
}
