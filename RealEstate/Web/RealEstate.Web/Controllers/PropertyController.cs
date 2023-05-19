namespace RealEstate.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

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
            // var test = this.propertyService.GetTop10NewestSells();

            var regionsDownTowns = new List<RegionDownTown>();
            var regions = this.regionService.Get<RegionViewModel>();

            foreach (var region in regions)
            {
                if (region.DownTown == null)
                {
                    continue;
                }

                regionsDownTowns.Add(new RegionDownTown { Id = region.Id, Name = region.Name });
                regionsDownTowns.Add(new RegionDownTown { Id = region.DownTown.Id, Name = region.DownTown.Name });
            }

            return this.View(new AddPropertyModel
            {
                PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>(),
                Regions = this.regionService.Get<RegionViewModel>(),
                RegionDownTown = regionsDownTowns,
                BuildingTypes = this.buildingTypeService.Get<BuildingTypeModel>(),
            });
        }

        public IActionResult GetDistrictsByDownTownId(string id)
        {
            var districts = this.districtService.GetDistrictByDownTownId<DistrictModel>(id);

            return this.Json(new { data = districts });
        }

        [HttpPost]
        public IActionResult GetPlacesByRegionId(string id)
        {
            var places = this.placeService.GetPlacesByRegionId<PlaceViewModel>(id);

            if (places == null || places.Count() == 0)
            {
                return this.GetDistrictsByDownTownId(id);
            }

            return this.Json(new { data = places });
        }

        [HttpPost]
        public IActionResult Add(AddPropertyModel property)
        {
            this.propertyService.Add(property);
            return this.Redirect("/");
        }
    }
}
