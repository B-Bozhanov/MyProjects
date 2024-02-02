namespace RealEstate.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.PropertyTypes;
    using RealEstate.Web.ViewModels.Search;

    using static RealEstate.Common.GlobalConstants.Properties;

    public class PropertyController : BaseController
    {
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly IBuildingTypeService buildingTypeService;
        private readonly IConditionService conditionService;
        private readonly IDetailService detailService;
        private readonly IEquipmentService equipmentService;
        private readonly IHeatingService heatingService;
        private readonly ILocationService locationService;
        private readonly IPaginationService paginationService;
        private readonly IPropertyService propertyService;
        private readonly IPopulatedPlaceService populatedPlaceService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly IImageService imageService;
        private readonly IConfiguration configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public PropertyController(IBackgroundJobClient backgroundJobClient, 
            IBuildingTypeService buildingTypeService, 
            IConditionService conditionService, 
            IDetailService detailService, 
            IEquipmentService equipmentService, 
            IHeatingService heatingService, 
            ILocationService locationService, 
            IPaginationService paginationService, 
            IPropertyService propertyService, 
            IPopulatedPlaceService populatedPlaceService, 
            IPropertyTypeService propertyTypeService,
            IImageService imageService,
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.backgroundJobClient = backgroundJobClient;
            this.buildingTypeService = buildingTypeService;
            this.conditionService = conditionService;
            this.detailService = detailService;
            this.equipmentService = equipmentService;
            this.heatingService = heatingService;
            this.locationService = locationService;
            this.paginationService = paginationService;
            this.propertyService = propertyService;
            this.populatedPlaceService = populatedPlaceService;
            this.propertyTypeService = propertyTypeService;
            this.imageService = imageService;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("/add")]
        public async Task<IActionResult> AddAsync() => this.View(await this.SetCollectionsAsync(new PropertyInputModel()));

        [HttpPost]
        [Route("/add")]
        public async Task<IActionResult> AddAsync(PropertyInputModel property)
        {
            var errors = this.propertyService.PropertyValidator(property);
            this.AddModelStateErrors(errors);

            if (!this.ModelState.IsValid)
            {
                return this.View(await this.SetCollectionsAsync(property));
            }
                        
            await this.propertyService.AddAsync(property, this.UserId);

            return this.RedirectToAction(nameof(this.Success));
        }

        [HttpGet]
        [Route("/edit")]
        public async Task<IActionResult> EditAsync(int propertyId)
        {
            var editModel = await this.propertyService.GetByIdWithExpiredUserPropertiesAsync<PropertyEditViewModel>(propertyId, this.UserId);

            if (editModel == null)
            {
                //TODO: Not found call administrator!
                return this.NotFound("Not found from EditGet");
            }

            editModel.BuildingType.IsChecked = true;
            editModel.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
            editModel.Locations = this.locationService.Get<LocationViewModel>();
            editModel.BuildingTypes = this.buildingTypeService.GetAll();
            editModel.Images = await this.imageService.GetAllByPropertyIdAsync(editModel.Id);

            foreach (var buildingType in editModel.BuildingTypes)
            {
                if (buildingType.Id == editModel.BuildingType.Id)
                {
                    buildingType.IsChecked = true;
                }
            }

            editModel.LocationId = editModel.PopulatedPlace.Location.Id;

            return this.View(editModel);
        }

        [HttpPost]
        [Route("/edit")]
        public async Task<IActionResult> EditAsync(PropertyEditViewModel editModel)
        {
            if (!await this.propertyService.IsUserProperty(editModel.Id, this.UserId))
            {
                //TODO: Not found call administrator!
                return this.NotFound("This is not your Property");
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

            await this.propertyService.EditAsync(editModel);

            var returnUrlCookieValue = this.Request.Cookies["ReturnUrl"];

            if (returnUrlCookieValue == null || this.propertyService.GetAllExpiredUserPropertiesCount(this.UserId) == 0)
            {
                return this.RedirectToMyActiveProperties();
            }

            return this.Redirect(returnUrlCookieValue);
        }

        [HttpPost]
        public IActionResult GetPopulatedPlaces(int id)
        {
            var populatedPlaces = this.populatedPlaceService.GetPopulatedPlacesByLocationId<PopulatedPlaceViewModel>(id);

            return this.Json(new { data = populatedPlaces });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(int optionId, int page = 1)
        {
            var key = this.configuration["ApiKeys:ImgBBApiKey"];
            var key2 = this.configuration["ApiKeys:SendGridApiKey"];

            try
            {
                var allPropertiesCount = this.propertyService.GetAllActiveCount();
                var paginationModel = this.paginationService.CreatePagination(allPropertiesCount, PropertiesPerPage, page, this.ControllerName(nameof(PropertyController)), nameof(this.Index));
                var searchModel = new SearchViewModel
                {
                    AllProperties = this.propertyService.GetAllByOptionIdPerPage(optionId, paginationModel.CurrentPage),
                    Locations = this.locationService.Get<LocationViewModel>(),
                };

                searchModel.CurrentOptionType = searchModel.OptionTypeModels.First(o => (int)o == optionId);

                this.ViewBag.Pager = paginationModel;

                return this.View(searchModel);
            }
            catch (Exception ex)
            {
                //TODO: Redirect to friedly error page
                return this.Content(ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/Property/Single")]
        public async Task<IActionResult> PropertySingle(int id)
        {
            var propertyModel = await this.propertyService.GetByIdAsync<PropertyViewModel>(id);
            return this.View(propertyModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Search(SearchViewModel searchModel, int page = 1)
        {
            try
            {
                var properties = await this.propertyService.SearchAsync(searchModel);

                var paginationModel = this.paginationService.CreatePagination(properties.Count(), PropertiesPerPage, page, this.ControllerName(nameof(PropertyController)), nameof(this.Search));

                this.ViewBag.Pager = paginationModel;
                return View(properties);
            }
            catch (System.Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return this.View(searchModel);
            }
        }

        public IActionResult Success()
        {
            return this.View();
        }

        private void AddModelStateErrors(Dictionary<string, List<string>> errorsCollection)
        {
            foreach (var errorCollection in errorsCollection)
            {
                foreach (var item in errorCollection.Value)
                {
                    this.ModelState.AddModelError($"{errorCollection.Key}", item);
                }
            }
        }

        private async Task<PropertyInputModel> SetCollectionsAsync(PropertyInputModel property)
        {
            property.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
            property.Locations = this.locationService.Get<LocationViewModel>();
            property.BuildingTypes = this.buildingTypeService.GetAll();
            property.Heatings = await this.heatingService.GetAllAsync();
            property.Details = await this.detailService.GetAllAsync();
            property.Equipments = await this.equipmentService.GetAllAsync();
            property.Conditions = await this.conditionService.GetAllAsync();

            return property;
        }
    }
}
