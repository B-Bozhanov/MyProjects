namespace RealEstate.Services.RegionScraperService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Location
    {
        public Location()
        {
            this.PopulatedPlaces = new();
        }

        public string Name { get; set; }

        public List<PopulatedPlace> PopulatedPlaces { get; private set; } = null!;

        internal void AddPopulatedPlace(string placeName)
        {
            if (!this.PopulatedPlaces.Any(x => x.Name == placeName))
            {
                this.PopulatedPlaces.Add(new PopulatedPlace(placeName));
            }
        }

        internal Location Parse(IEnumerable<string> locations, Dictionary<string, List<string>> downTownsDistricts)
        {
            foreach (var location in locations)
            {
                var locationParts = location.Split(", ", StringSplitOptions.RemoveEmptyEntries);

                var villageName = locationParts[0];
                var townShipName = locationParts[1].Replace("община ", "гр.");
                var locationName = locationParts[2];

                this.Name ??= locationName;

                if (locationParts[2] == "област София (столица)")
                {
                    this.AddPopulatedPlace(locationParts[0]);
                }
                else
                {
                    this.AddPopulatedPlace(villageName);
                    this.AddPopulatedPlace(townShipName);

                    //if (downTownsDistricts.ContainsKey(this.Name))
                    //{
                    //    var test = downTownsDistricts[this.Name];
                    //    this.PopulatedPlaces.AddRange(downTownsDistricts[this.Name]);
                    //}
                }
            }

            return this;
        }
    }
}
