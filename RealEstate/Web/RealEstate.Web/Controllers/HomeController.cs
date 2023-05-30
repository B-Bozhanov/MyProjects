namespace RealEstate.Web.Controllers
{
    using System;
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels;
    using RealEstate.Web.ViewModels.Property;

    using static RealEstate.Common.GlobalConstants.Properties;

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyService propertyService;

        public HomeController(
            ILogger<HomeController> logger,
            IPropertyService propertyService)
        {
            this._logger = logger;
            this.propertyService = propertyService;
        }

        public IActionResult Index()
        {
            return this.View(new PropertyIntroViewModel
            {
                GetAllCount = this.propertyService.GetAllCount(),
                Newest = this.propertyService.GetTopNewest(TopNewest),
                MostExpensive = this.propertyService.GetTopMostExpensive(TopMostExpensive),
            });
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
