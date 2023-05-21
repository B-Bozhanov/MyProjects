namespace RealEstate.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Services.RegionScraperService.Models;

    public interface IRegionScraperService
    {
        public Task<IEnumerable<Location>> GetRegionsAsync(string country = null);

        public Task<string> GetAllAsJason();
    }
}
