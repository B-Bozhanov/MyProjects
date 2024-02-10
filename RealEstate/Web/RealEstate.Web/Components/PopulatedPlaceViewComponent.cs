namespace RealEstate.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.PopulatedPlaces;

    public class PopulatedPlaceViewComponent : ViewComponent
    {
        private readonly IPopulatedPlaceService populatedPlaceService;

        public PopulatedPlaceViewComponent(IPopulatedPlaceService populatedPlaceService)
        {
            this.populatedPlaceService = populatedPlaceService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int locationId)
        {
            var model = this.populatedPlaceService.GetPopulatedPlacesByLocationId<PopulatedPlaceViewModel>((int)locationId);
            return this.View(model);

        }
    }
}
