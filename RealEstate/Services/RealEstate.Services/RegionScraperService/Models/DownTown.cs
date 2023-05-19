namespace RealEstate.Services.RegionScraperService.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class DownTown
    {
        public DownTown(string name)
        {
            this.Name = name;
            this.Districts = new();
        }

        public string Name { get; set; }

        public HashSet<District> Districts { get; private set; } = null!;

        internal void Add(string district)
        {
            if (!this.Districts.Any(x => x.Name == district))
            {
                this.Districts.Add(new District(district));
            }
        }

        internal void AddRange(IEnumerable<string> districts)
        {
            foreach (var district in districts)
            {
                this.Add(district);
            }
        }
    }
}
