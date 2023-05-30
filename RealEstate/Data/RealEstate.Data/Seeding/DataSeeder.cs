namespace RealEstate.Data.Seeding
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json;

    using RealEstate.Common;

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
