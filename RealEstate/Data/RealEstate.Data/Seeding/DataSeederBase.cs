namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    public abstract class DataSeederBase 
    {
        public IEnumerable<T> GetDataFromJson<T>(string fileName, IServiceProvider serviceProvider)
        {
            var environment = serviceProvider.GetService<IWebHostEnvironment>();
            var rootPath = environment.WebRootPath;
            var test = File.ReadAllText($"{rootPath}/dataToSeed/{fileName}s.json");
            //var json = File.ReadAllText($"{GlobalConstants.SeedDataPath}{fileName}s.json");

            var list = JsonConvert.DeserializeObject<IEnumerable<T>>(test);

            return list;
        }
    }
}
