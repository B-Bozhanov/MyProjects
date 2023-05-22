namespace RealEstate.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public class PropertyController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly ILocationService locationService;
        private readonly IBuildingTypeService buildingTypeService;
        private readonly IPopulatedPlaceService populatedPlaceService;

        public PropertyController(
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            ILocationService regionService,
            IBuildingTypeService buildingTypeService,
            IPopulatedPlaceService placeService)
        {
            this.propertyService = propertyService;
            this.propertyTypeService = propertyTypeService;
            this.locationService = regionService;
            this.buildingTypeService = buildingTypeService;
            this.populatedPlaceService = placeService;
        }

        public IActionResult Add()
        {
            return this.View(new AddPropertyViewModel
            {
                PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>(),
                Locations = this.locationService.Get<LocationViewModel>(),
                BuildingTypes = this.buildingTypeService.Get<BuildingTypeModel>(),
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPropertyViewModel property)
        {
            await this.propertyService.Add(property);
            return this.Redirect("/");
        }

        [HttpPost]
        public IActionResult GetPopulatedPlaces(int id)
        {
            var populatedPlaces = this.populatedPlaceService.GetPopulatedPlacesByLocationId<PopulatedPlaceViewModel>(id);

            return this.Json(new { data = populatedPlaces });
        }
    }
}
