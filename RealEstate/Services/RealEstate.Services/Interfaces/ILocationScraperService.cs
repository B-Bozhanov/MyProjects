namespace RealEstate.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Services.RegionScraperService.Models;

    public interface ILocationScraperService
    {
        public Task<IEnumerable<Location>> GetLocationsAsync(string country = null);

        public Task<string> GetAllAsJason();
    }
}
