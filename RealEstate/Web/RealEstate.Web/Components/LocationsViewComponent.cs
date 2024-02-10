namespace RealEstate.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.Locations;

    public class LocationsViewComponent : ViewComponent
    {
        private readonly ILocationService locationService;

        public LocationsViewComponent(ILocationService locationService)
        {
            this.locationService = locationService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = locationService.Get<LocationViewModel>();

            return this.View(model);
        }
    }
}
