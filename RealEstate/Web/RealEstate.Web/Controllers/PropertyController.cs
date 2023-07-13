namespace RealEstate.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

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
        private readonly IBackgroundJobClient backgroundJobClient;

        public PropertyController(
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            ILocationService locationService,
            IBuildingTypeService buildingTypeService,
            IPopulatedPlaceService placeService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IBackgroundJobClient backgroundJobClient)
        {
            this.propertyService = propertyService;
            this.propertyTypeService = propertyTypeService;
            this.locationService = locationService;
            this.buildingTypeService = buildingTypeService;
            this.populatedPlaceService = placeService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.backgroundJobClient = backgroundJobClient;
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
            => this.View(await this.SetCollectionsAsync(new PropertyInputModel()));

        [HttpPost]
        public async Task<IActionResult> Add(PropertyInputModel property)
        {
            this.PropertyValidator(property);

            if (!this.ModelState.IsValid)
            {
                return this.View(await this.SetCollectionsAsync(property));
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.propertyService.AddAsync(property, user);

            return this.RedirectToAction(nameof(this.Success));
        }
        
        [HttpPost]
        public IActionResult Search(SearchViewModel searchModel)
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult PropertySingle(int id)
            => this.View(this.propertyService.GetByIdAsync<PropertyViewModel>(id));

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
            var editModel = await this.propertyService.GetByIdAsync<PropertyEditViewModel>(propertyId, this.UserId);

            if (editModel == null)
            {
                return this.NotFound(editModel);
            }

            editModel.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
            editModel.Locations = this.locationService.Get<LocationViewModel>();
            editModel.BuildingTypes = this.buildingTypeService.GetAll();

            editModel.LocationId = editModel.PopulatedPlace.Location.Id;

            return this.View(editModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PropertyEditViewModel editModel)
        {
            if (!await this.propertyService.IsUserProperty(editModel.Id, this.UserId))
            {
                return this.NotFound();
            }

            if (editModel.BuildingTypes.Where(b => b.IsChecked).Count() > 1)
            {
                this.ModelState.AddModelError("", "Canot check more than one building type!");
            }

            if (!this.ModelState.IsValid)
            {
                editModel.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
                editModel.Locations = this.locationService.Get<LocationViewModel>();
                editModel.BuildingTypes = this.buildingTypeService.GetAll();

                editModel.PopulatedPlace = this.populatedPlaceService.GetPopulatedPlacesByProperty<PopulatedPlaceViewModel>(editModel.PopulatedPlaceId);
                editModel.LocationId = editModel.PopulatedPlace.Location.Id;

                return this.View(editModel);
            }

            if (editModel.ExpirationDays != 0)
            {
                editModel.IsExpirationDaysModified = true;
            }

            await this.propertyService.Edit(editModel);

            return this.RedirectToMyProperties();
        }

        private void PropertyValidator(PropertyInputModel property)
        {
            if (property.BuildingTypes.Where(b => b.IsChecked).Count() > 1)
            {
                this.ModelState.AddModelError("", "Canot check more than one building type!");
            }
            else if (property.BuildingTypes.All(b => !b.IsChecked))
            {
                this.ModelState.AddModelError("", "Building type is required!");
            }
        }

        private async Task<PropertyInputModel> SetCollectionsAsync(PropertyInputModel property)
        {
            property.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
            property.Locations = this.locationService.Get<LocationViewModel>();
            property.BuildingTypes = this.buildingTypeService.GetAll();
            property.Conditions = await this.conditionService.GetAllAsync();
            property.Heatings = await this.heatingService.GetAllAsync();
            property.Details = await this.detailService.GetAllAsync();
            property.Equipments = await this.equipmentService.GetAllAsync();

            return property;
        }
    }
}
