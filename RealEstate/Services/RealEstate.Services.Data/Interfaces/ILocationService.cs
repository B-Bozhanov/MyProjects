namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Web.ViewModels.Locations;

    public interface ILocationService 
    {
        public void SaveToFile(string file);

        public IEnumerable<T> Get<T>();

        IEnumerable<LocationViewModel> GetLocations();
    }
}
