namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILocationService 
    {
        public void SaveToFile(string file);

        public IEnumerable<T> Get<T>();
    }
}
