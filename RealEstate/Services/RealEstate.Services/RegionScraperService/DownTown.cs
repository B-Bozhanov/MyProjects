namespace RealEstate.Services.RegionScraperService
{
    using System.Collections.Generic;

    public class DownTown
    {
        internal DownTown(string name)
        {
            this.Name = name;
            this.Districts = new();
        }

        public string Name { get; set; }

        public HashSet<string> Districts { get; private set; } = null!;

        public void Add(string district)
        {
            if (!this.Districts.Contains(district))
            {
                this.Districts.Add(district);
            }
        }

        public void AddRange(IEnumerable<string> districts)
        {
            foreach (var district in districts)
            {
                this.Add(district);
            }
        }
    }
}
