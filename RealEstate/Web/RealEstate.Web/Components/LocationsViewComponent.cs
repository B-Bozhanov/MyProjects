namespace RealEstate.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.Property;

    public class LocationsViewComponent : ViewComponent
    {
        private readonly ILocationService locationService;

        public LocationsViewComponent(ILocationService locationService)
        {
            this.locationService = locationService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(IBasePropertyModel model)
        {
            model.Locations = this.locationService.Get<LocationViewModel>();
            return this.View(model);
        }
    }
}
