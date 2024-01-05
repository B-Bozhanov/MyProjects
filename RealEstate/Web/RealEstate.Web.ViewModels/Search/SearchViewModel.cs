namespace RealEstate.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.Property;

    public class SearchViewModel : IMapFrom<RealEstate.Data.Models.Property>
    {
        public SearchViewModel()
        {
            this.OptionTypeModels = new List<OptionType>
            {
                OptionType.All,
                OptionType.ForSale,
                OptionType.ForRent,
                OptionType.OldToNew,
                OptionType.NewToOld,
                OptionType.PriceDesc,
                OptionType.PriceAsc,
            };
        }

        public string KeyWord { get; init; }

        [Display(Name = "Location")]
        public int? LocationId { get; init; }

        [Display(Name = "Populated place")]
        public int? PopulatedPlaceId { get; init; }

        public OptionType? Type { get; init; }

        public int? BedRooms { get; init; }

        public int? Garages { get; init; }

        public int? BathRooms { get; init; }

        public decimal? MinPrice { get; init; }

        public decimal? MaxPrice { get; init; }

        public OptionType CurrentOptionType { get; set; }

        public IEnumerable<LocationViewModel> Locations { get; init; }

        public IEnumerable<LocationViewModel> PopulatedPlaces { get; init; }

        public IEnumerable<PropertyViewModel> AllProperties { get; init; }

        public IEnumerable<OptionType> OptionTypeModels { get; init; }
    }
}
