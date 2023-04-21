namespace RealEstate.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface IImportService
    {
        void Import(IFormFile file);
    }
}
