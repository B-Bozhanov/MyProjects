namespace RealEstate.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

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
        private readonly IConditionService conditionService;
        private readonly IHeatingService heatingService;
        private readonly IEquipmentService equipmentService;
        private readonly IDetailService detailService;
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

        [HttpGet]
        public IActionResult Index(int optionId)
        {
            var searchModel = new SearchViewModel
            {
                AllProperties = this.propertyService.GetAllByOptionId(optionId),
                Locations = this.locationService.Get<LocationViewModel>(),
            };

            searchModel.CurrentOptionType = searchModel.OptionTypeModels.First(o => (int)o == optionId);

            return this.View(searchModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add() 
         => this.View(await this.propertyService.SetCollectionsAsync(new AddPropertyInputModel()));
        
        [HttpPost]
        public async Task<IActionResult> Add(AddPropertyInputModel property)
        {
            this.PropertyValidator(property);

            if (!this.ModelState.IsValid)
            {
                return this.View(await this.propertyService.SetCollectionsAsync(property));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.propertyService.Add(property, user);

            return this.RedirectToAction(nameof(this.Success));
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel searchModel)
        {
            return this.View();
        }

        public IActionResult PropertySingle(int id)
            => this.View(this.propertyService.GetById<PropertyViewModel>(id));

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

        [HttpGet]
        public async Task<IActionResult> Edit(int propertyId)
        {
            var editModel = this.propertyService.GetById<EditViewModel>(propertyId, this.UserId);

            if (editModel == null)
            {
                return this.NotFound();
            }

            editModel.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
            editModel.Locations = this.locationService.Get<LocationViewModel>();
            editModel.BuildingTypes = this.buildingTypeService.Get<BuildingTypeViewModel>();

            editModel.LocationId = editModel.PopulatedPlace.Location.Id;

            return this.View(editModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel editModel)
        {
            if (editModel.BuildingTypes.Where(b => b.IsChecked).Count() > 1)
            {
                this.ModelState.AddModelError("", "Canot check more than one building type!");
            }
            if (editModel.BuildingTypes.All(b => !b.IsChecked))
            {
                this.ModelState.AddModelError("", "Building type is required!");
            }

            if (!this.ModelState.IsValid)
            {
                editModel.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
                editModel.Locations = this.locationService.Get<LocationViewModel>();
                editModel.BuildingTypes = this.buildingTypeService.Get<BuildingTypeViewModel>();

                editModel.PopulatedPlace = this.populatedPlaceService.GetPopulatedPlacesByProperty<PopulatedPlaceViewModel>(editModel.PopulatedPlaceId);
                editModel.LocationId = editModel.PopulatedPlace.Location.Id;

                return this.View(editModel);
            }

            await this.propertyService.Edit(editModel);

            return this.RedirectToMyProperties();
        }

        private void PropertyValidator(AddPropertyInputModel property)
        {
            if (property.BuildingTypes.Where(b => b.IsChecked).Count() > 1)
            {
                this.ModelState.AddModelError("", "Canot check more than one building type!");
            }
            else if (property.BuildingTypes.All(b => !b.IsChecked))
            {
                this.ModelState.AddModelError("", "Building type is required!");
            }

            if (property.Conditions.Where(c => c.IsChecked).Count() > 2)
            {
                this.ModelState.AddModelError("", "Canot check more than one building type!");
            }

            if (property.Equipments.Where(e => e.IsChecked).Count() > 1)
            {
                this.ModelState.AddModelError("", "Canot check more than one building type!");
            }
        }
    }
}
