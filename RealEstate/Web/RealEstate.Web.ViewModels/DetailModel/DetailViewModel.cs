namespace RealEstate.Web.ViewModels.DetailModel
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class DetailViewModel : IMapFrom<Detail>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}
