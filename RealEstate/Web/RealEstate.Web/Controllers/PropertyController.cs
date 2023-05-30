namespace RealEstate.Web.Controllers
{
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
            ILocationService regionService,
            IBuildingTypeService buildingTypeService,
            IPopulatedPlaceService placeService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.propertyService = propertyService;
            this.propertyTypeService = propertyTypeService;
            this.locationService = regionService;
            this.buildingTypeService = buildingTypeService;
            this.populatedPlaceService = placeService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Authorize]
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
        [Authorize]
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
            return this.Redirect("/");
        }

        public IActionResult PropertySingle(int id)
            => this.View(this.propertyService.GetById(id));

        public IActionResult PropertyGrid()
            => this.View(new PropertyIntroViewModel
            {
                All = this.propertyService.GetAll(),
            });

        [HttpPost]
        public IActionResult GetPopulatedPlaces(int id)
        {
            var populatedPlaces = this.populatedPlaceService.GetPopulatedPlacesByLocationId<PopulatedPlaceViewModel>(id);

            return this.Json(new { data = populatedPlaces });
        }
    }
}
