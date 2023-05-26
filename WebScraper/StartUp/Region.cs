namespace StartUp
{
    public class Region
    {
        public Region()
        {
            Places = new();
        }

        public string? Name { get; set; }

        public DownTown? DownTown { get; set; }

        public HashSet<string> Places { get; private set; } = null!;

        public void AddPlace(string placeName)
        {
            if (!Places.Contains(placeName))
            {
                this.Places.Add(placeName);
            }
        }

        public Region Parse(IEnumerable<string> places, Dictionary<string, List<string>> downTownsDistricts)
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
                    this.Places.Add(placeParts[0]);
                }
                else
                {
                    this.Places.Add(villageName);
                    this.Places.Add(townShipName);

                    this.DownTown ??= new DownTown(downTownName);

                    if (downTownsDistricts.ContainsKey(downTownName))
                    {
                        this.DownTown.AddRange(downTownsDistricts[downTownName]);
                    }

                    if (!this.DownTown.Districts.Contains("Център"))
                    {
                        this.DownTown.Add("Център");
                    }
                }
            }

            return this;
        }
    }
}