namespace RealEstate.Web.ViewModels.BuildingTypeModel
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class BuildingTypeModel : IMapFrom<BuildingType>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}
