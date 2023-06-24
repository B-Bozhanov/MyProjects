namespace RealEstate.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using RealEstate.Common;
    using RealEstate.Data.Models;

    public abstract class DataSeeder 
    {
        public IEnumerable<T> GetDataFromJson<T>(string fileName)
        {
            var json = File.ReadAllText($"{GlobalConstants.SeedDataPath}{fileName}s.json");

            var list = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return list;
        }
    }
}
