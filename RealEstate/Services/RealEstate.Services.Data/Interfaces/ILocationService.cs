namespace RealEstate.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface ILocationService : IService
    {
        public void SaveToFile(string file);
    }
}
