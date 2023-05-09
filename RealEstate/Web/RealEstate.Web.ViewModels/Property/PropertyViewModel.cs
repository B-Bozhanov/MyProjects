namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Districts;
    using RealEstate.Web.ViewModels.Places;

    public class PropertyViewModel
    {
        public decimal? Prize { get; init; }

        public PropertyTypeViewModel Type { get; init; } = null!;

        public PlaceModel Place { get; init; } = null!;

        public DistrictModel District { get; init; } = null!;

        public int Size { get; init; }

        public Dictionary<string, int> Extras { get; init; }

        public Image Image { get; init; }
    }
}
