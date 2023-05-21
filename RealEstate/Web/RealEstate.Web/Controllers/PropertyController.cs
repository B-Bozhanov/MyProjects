namespace RealEstate.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Win32;

    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.Districts;
    using RealEstate.Web.ViewModels.Places;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.PropertyTypes;
    using RealEstate.Web.ViewModels.Regions;

    public class PropertyController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly IRegionService regionService;
        private readonly IBuildingTypeService buildingTypeService;
        private readonly IDistrictService districtService;
        private readonly IPlaceService placeService;

        public PropertyController(
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            IRegionService regionService,
            IBuildingTypeService buildingTypeService,
            IDistrictService districtService,
            IPlaceService placeService)
        {
            this.propertyService = propertyService;
            this.propertyTypeService = propertyTypeService;
            this.regionService = regionService;
            this.buildingTypeService = buildingTypeService;
            this.districtService = districtService;
            this.placeService = placeService;
        }

        public IActionResult Add()
        {
            return this.View(new AddPropertyViewModel
            {
                PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>(),
                Regions = this.regionService.Get<RegionViewModel>(),
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
        public IActionResult GetPlacesByRegionId(int id)
        {
            var places = this.placeService.GetPlacesByRegionId<PlaceViewModel>(id);

            if (places.Count() == 0)
            {
                return this.GetDistrictsByDownTownId(id);
            }

            return this.Json(new { data = places });
        }

        private IActionResult GetDistrictsByDownTownId(string id)
        {
            var districts = this.districtService.GetDistrictByDownTownId<DistrictModel>(id);

            if (districts.Count() == 1)
            {
                districts.Add(new DistrictModel { Name = "TODO: Search for districts!" });
            }

            return this.Json(new { data = districts });
        }
    }
}
