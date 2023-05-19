namespace RealEstate.Services.RegionScraperService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Region
    {
        public Region()
        {
            this.Places = new();
        }

        public string Name { get; set; }

        public DownTown DownTown { get; set; }

        public HashSet<Place> Places { get; private set; } = null!;

        internal void AddPlace(string placeName)
        {
            if (!this.Places.Any(x => x.Name == placeName))
            {
                this.Places.Add(new Place(placeName));
            }
        }

        internal Region Parse(IEnumerable<string> places, Dictionary<string, List<string>> downTownsDistricts)
        {
            foreach (var place in places)
            {
                var placeParts = place.Split(", ", StringSplitOptions.RemoveEmptyEntries);

                var villageName = placeParts[0];
                var townShipName = placeParts[1].Replace("община ", "гр.");
                var regionName = placeParts[2];
                var downTownName = placeParts[2].Replace("област ", "гр.");

                this.Name ??= regionName;

                if (placeParts[2] == "област София (столица)")
                {
                    this.AddPlace(placeParts[0]);
                }
                else
                {
                    this.AddPlace(villageName);
                    this.AddPlace(townShipName);

                    this.DownTown ??= new DownTown(downTownName);

                    if (downTownsDistricts.ContainsKey(downTownName))
                    {
                        this.DownTown.AddRange(downTownsDistricts[downTownName]);
                    }

                    if (!this.DownTown.Districts.Any(x => x.Name == "Център"))
                    {
                        this.DownTown.Add("Център");
                    }
                }
            }

            return this;
        }
    }
}
