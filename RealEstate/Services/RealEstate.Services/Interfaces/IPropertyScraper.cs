namespace RealEstate.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IPropertyScraper
    {
        public Task<string> GetAsJson(string url);
    }
}
