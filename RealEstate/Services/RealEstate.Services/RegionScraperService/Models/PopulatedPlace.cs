namespace RealEstate.Services.RegionScraperService.Models
{
    public class PopulatedPlace
    {
        public PopulatedPlace(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
