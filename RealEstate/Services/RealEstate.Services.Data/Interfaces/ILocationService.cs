namespace RealEstate.Services.Data.Interfaces
{
    public interface ILocationService : IService
    {
        public void SaveToFile(string file);
    }
}
