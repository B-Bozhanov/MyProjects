namespace RealEstate.App.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Models.ErrorViewModels;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}