namespace RealEstate.Models.ImportModels
{
    public class BuildingTypeModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsSelected { get; set; } = false;

        public bool IsDisabled { get; set; }
    }
}