namespace RealEstate.Models.ImportViewModels
{
    public class BuildingTypeViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}