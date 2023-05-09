namespace RealEstate.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Interfaces;
    using RealEstate.Web.ViewModels.RegionScraper;

    public class UpdateRegionController : Controller
    {
        private readonly IRegionScraperService scraper;

        public UpdateRegionController(IRegionScraperService scraper)
        {
            this.scraper = scraper;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Update()
        {
            return this.View(new UpdateRegionViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "creator")]
        public async Task<IActionResult> Update(string downTownsUrl, string regionsUrl)
        {
            var regions = await this.scraper.GetRegionsAsync(downTownsUrl, regionsUrl);

            this.scraper.SaveAsJson(new string("TODO"), "bgRegions", regions);

            return this.RedirectToAction(nameof(this.Succsses));
        }

        public IActionResult Succsses()
        {
            return this.View();
        }
    }
}
