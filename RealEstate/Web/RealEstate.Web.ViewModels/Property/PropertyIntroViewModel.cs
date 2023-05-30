namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;

    public class PropertyIntroViewModel
    {
        public int GetAllCount { get; init; }

        public IEnumerable<PropertyViewModel> Newest { get; init; }

        public IEnumerable<PropertyViewModel> MostExpensive { get; init; }

        public IEnumerable<PropertyViewModel> All { get; set; }
    }
}
