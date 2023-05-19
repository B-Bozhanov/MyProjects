namespace RealEstate.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this._logger = logger;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Sales()
        {
            return this.View();
        }

        public IActionResult Rentals()
        {
            return this.View();
        }

        public IActionResult NewConstructions()
        {
            return this.View();
        }

        public IActionResult Agencies()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
