namespace RealEstate.Services
{
    using System.Text;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    using RealEstate.Models.ImportModels;
    using RealEstate.Services.Interfaces;

    public class ImportService : IImportService
    {
        private readonly IPropertyService propertyService;

        public ImportService(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        public void Import(IFormFile file)
        {
            var jsonStr = GetFileAsString(file);

            if (string.IsNullOrWhiteSpace(jsonStr))
            {
                throw new InvalidOperationException("Json can not be empty!");
            }

            var jsonProps = JsonConvert.DeserializeObject<IEnumerable<AddPropertyModel>>(jsonStr);

           
            foreach (var jsonProp in jsonProps!)
            {
                this.propertyService.Add(jsonProp);
            }
        }

        private static string GetFileAsString(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();

            return Encoding.UTF8.GetString(fileBytes);
        }
    }
}
