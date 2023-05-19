namespace RealEstate.Web.ViewModels.Districts
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class DistrictModel : IMapFrom<District>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
