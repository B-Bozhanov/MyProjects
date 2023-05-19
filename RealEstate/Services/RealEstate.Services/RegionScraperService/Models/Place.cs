namespace RealEstate.Services.RegionScraperService.Models
{
    public class Place
    {
        public Place(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
