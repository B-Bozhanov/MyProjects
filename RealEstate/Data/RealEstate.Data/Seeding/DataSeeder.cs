namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    using RealEstate.Common;
    using RealEstate.Data.Models;

    public abstract class DataSeeder 
    {
        public IEnumerable<T> GetDataFromJson<T>(string fileName, IServiceProvider serviceProvider)
        {
            var environment = serviceProvider.GetService<IHostingEnvironment>();
            var rootPath = environment.WebRootPath;
            var test = File.ReadAllText($"{rootPath}/dataToSeed/{fileName}s.json");
            //var json = File.ReadAllText($"{GlobalConstants.SeedDataPath}{fileName}s.json");

            var list = JsonConvert.DeserializeObject<IEnumerable<T>>(test);

            return list;
        }
    }
}
