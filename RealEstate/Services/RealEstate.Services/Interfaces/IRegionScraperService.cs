namespace RealEstate.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RealEstate.Services.RegionScraperService;

    public interface IRegionScraperService
    {
        public Task<IEnumerable<Region>> GetRegionsAsync(string downTownsUrl, string regionsUrl);

        public void SaveAsJson(string path, string fileName, IEnumerable<Region> regions);
    }
}
