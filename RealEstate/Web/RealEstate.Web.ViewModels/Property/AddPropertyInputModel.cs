namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Districts;
    using RealEstate.Web.ViewModels.PropertyTypes;
    using RealEstate.Web.ViewModels.Regions;

    public class AddPropertyInputModel
    {
        public decimal? Prize { get; init; }

        public PropertyTypeViewModel Type { get; init; } = null!;

        public RegionViewModel Place { get; init; } = null!;

        public DistrictModel District { get; init; } = null!;

        public int Size { get; init; }

        public Dictionary<string, int> Extras { get; init; }

        public Image Image { get; init; }
    }
}
