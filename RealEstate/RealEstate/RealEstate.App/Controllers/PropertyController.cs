namespace RealEstate.App.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Models.ImportModels;
    using RealEstate.Services.Interfaces;
    public class PropertyController : Controller
    {
        private readonly IPropertyService propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        public IActionResult Add()
        {
            return this.View(new AddPropertyModel
            {
                PropertyTypes = this.propertyService.GetPropertiesTypes(),
                Places = this.propertyService.GetPlaces(),
                Districts = this.propertyService.GetDistricts(),
                BuildingTypes = this.propertyService.GetBuildingsTypes(),
            });
        }

        [HttpPost]
        public IActionResult Add(AddPropertyModel property)
        {
            this.propertyService.Add(property);
            return this.Redirect("/");
        }
    }
}
