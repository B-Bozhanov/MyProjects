namespace RealEstate.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.Xml;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.PropertyTypes;
    using RealEstate.Web.ViewModels.Search;

    public class PropertyController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly ILocationService locationService;
        private readonly IBuildingTypeService buildingTypeService;
        private readonly IPopulatedPlaceService populatedPlaceService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public PropertyController(
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            ILocationService locationService,
            IBuildingTypeService buildingTypeService,
            IPopulatedPlaceService placeService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.propertyService = propertyService;
            this.propertyTypeService = propertyTypeService;
            this.locationService = locationService;
            this.buildingTypeService = buildingTypeService;
            this.populatedPlaceService = placeService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index(int optionId)
        {
            var searchModel = new SearchViewModel
            {
                AllProperties = this.propertyService.GetAllByOptionId(optionId),
                Locations = this.locationService.Get<LocationViewModel>(),
            };

            searchModel.CurrentOptioType = searchModel.OptionTypeModels.First(o => (int)o == optionId);

            return this.View(searchModel);
        }

        public IActionResult Add()
        {
            return this.View(new AddPropertyInputModel
            {
                PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>(),
                Locations = this.locationService.Get<LocationViewModel>(),
                BuildingTypes = this.buildingTypeService.Get<BuildingTypeModel>(),
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPropertyInputModel property)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (this.signInManager.IsSignedIn(this.User))
            {
                // TODO:
            }

            if (user == null)
            {
                // TODO:
            }

            await this.propertyService.Add(property, user);

            return this.RedirectToAction(nameof(this.Success));
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel searchModel)
        {
            return this.View();
        }

        public IActionResult PropertySingle(int id)
            => this.View(this.propertyService.GetById(id));

        [HttpPost]
        public IActionResult GetPopulatedPlaces(int id)
        {
            var populatedPlaces = this.populatedPlaceService.GetPopulatedPlacesByLocationId<PopulatedPlaceViewModel>(id);

            return this.Json(new { data = populatedPlaces });
        }

        public IActionResult Success()
        {
            return this.View();
        }
    }
}
