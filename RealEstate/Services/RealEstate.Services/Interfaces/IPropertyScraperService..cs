namespace RealEstate.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IPropertyScraperService
    {
        public Task<string> GetAsJson(string url);
    }
}
