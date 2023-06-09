namespace RealEstate.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Common;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class UpdateRegionController : BaseController
    {
        private readonly IRegionScraperService scraper;
        private readonly ILocationService locationService;

        public UpdateRegionController(IRegionScraperService scraper, ILocationService regionService)
        {
            this.scraper = scraper;
            this.locationService = regionService;
        }

        //[HttpPost]
        public async Task<IActionResult> UpdateRegions()
        {
            var locations = await this.scraper.GetAllAsJason();

            this.locationService.SaveToFile(locations);

            return this.Content("Done");
            return this.RedirectToAction(nameof(this.Succsses));
        }

        public IActionResult Succsses()
        {
            return this.View();
        }
    }
}
