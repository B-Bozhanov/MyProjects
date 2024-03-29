﻿namespace RealEstate.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels;
    using RealEstate.Web.ViewModels.Property;

    using static RealEstate.Common.GlobalConstants.Properties;

    public class HomeController : BaseController
    {
        private readonly IPropertyGetService propertyGetService;

        public HomeController(
            IPropertyGetService propertyGetService)
        {
            this.propertyGetService = propertyGetService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return this.View(new PropertyIntroViewModel
            {
                GetAllCount = this.propertyGetService.GetAllActiveCount(),
                Newest = this.propertyGetService.GetTopNewest<PropertyViewModel>(TopNewest),
                MostExpensive = this.propertyGetService.GetTopMostExpensive<PropertyViewModel>(TopMostExpensive),
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
