namespace RealEstate.Web.ViewModels.Search
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Property;

    public class PropertyIndexModel
    {
        public PropertyIndexModel()
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
                OptionType.Test,
            };
        }

        [BindRequired]
        public SearchInputModel SearchInputModel { get; set; }

        public OptionType? CurrentOptionType { get; set; }

        public IEnumerable<PropertyViewModel> Properties { get; init; }

        public IEnumerable<OptionType> OptionTypeModels { get; init; }
    }
}
