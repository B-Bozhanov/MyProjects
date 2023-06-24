namespace RealEstate.Web.ViewModels.HeatingModel
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class HeatingViewModel : IMapFrom<Heating>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}
