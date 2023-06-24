namespace RealEstate.Web.ViewModels.ConditionModel
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class ConditionViewModel : IMapFrom<Condition>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}
