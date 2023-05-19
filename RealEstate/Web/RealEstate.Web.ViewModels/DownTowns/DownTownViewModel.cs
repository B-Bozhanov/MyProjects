namespace RealEstate.Web.ViewModels.DownTowns
{
    using System.Collections.Generic;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Districts;

    public class DownTownViewModel : IMapFrom<DownTown>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<DistrictModel> Districts { get; set; }
    }
}
