namespace RealEstate.Web.ViewModels.EquipmentModel
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class EquipmentViewModel : IMapFrom<Equipment>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}
