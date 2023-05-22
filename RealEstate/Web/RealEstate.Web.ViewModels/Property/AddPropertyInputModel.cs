namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public class AddPropertyInputModel
    {
        public decimal? Prize { get; init; }

        public PropertyTypeViewModel Type { get; init; } = null!;

        public LocationViewModel Location { get; init; } = null!;

        public int Size { get; init; }

        public Dictionary<string, int> Extras { get; init; }

        public Image Image { get; init; }
    }
}
