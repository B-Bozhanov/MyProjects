namespace RealEstate.Models.ImportModels
{
    public class BuildingTypeModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}