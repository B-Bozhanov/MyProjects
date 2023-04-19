namespace RealEstate.Models.ErrorViewModels
{
    using RealEstate.Models.DataModels;

    public class PropertyViewModel
    {
        public string Type { get; set; }

        public string Place { get; set; }

        public int Prize { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
