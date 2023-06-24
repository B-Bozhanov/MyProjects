namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.PropertyTypes;
    using RealEstate.Web.ViewModels.Search;

    public class PropertyService : IPropertyService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<PropertyType> propertyTypeRepository;
        private readonly IDeletableEntityRepository<BuildingType> buildingTypeRepository;
        private readonly IDeletableEntityRepository<UserContact> userContactsRepository;
        private readonly IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository;
        private readonly ILocationService locationService;
        private readonly IBuildingTypeService buildingTypeService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly IPopulatedPlaceService placeService;
        private readonly IConditionService conditionService;
        private readonly IHeatingService heatingService;
        private readonly IEquipmentService equipmentService;
        private readonly IDetailService detailService;
        private readonly IImageService imageService;

        public PropertyService(
              IDeletableEntityRepository<Property> propertyRepository,
              IDeletableEntityRepository<PropertyType> propertyTypeRepository,
              IDeletableEntityRepository<BuildingType> buildingTypeRepository,
              IDeletableEntityRepository<UserContact> userContactsRepository,
              IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository,
              ILocationService locationService,
              IBuildingTypeService buildingTypeService,
              IPropertyTypeService propertyTypeService,
              IPopulatedPlaceService placeService,
              IConditionService conditionService,
              IHeatingService heatingService,
              IEquipmentService equipmentService,
              IDetailService detailService,
              IImageService imageService)
        {
            this.propertyRepository = propertyRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.buildingTypeRepository = buildingTypeRepository;
            this.userContactsRepository = userContactsRepository;
            this.populatedPlaceRepository = populatedPlaceRepository;
            this.locationService = locationService;
            this.buildingTypeService = buildingTypeService;
            this.propertyTypeService = propertyTypeService;
            this.placeService = placeService;
            this.conditionService = conditionService;
            this.heatingService = heatingService;
            this.equipmentService = equipmentService;
            this.detailService = detailService;
            this.imageService = imageService;
        }

        public async Task Add(AddPropertyInputModel propertyModel, ApplicationUser user, [CallerMemberName] string import = null!)
        {
            var property = new Property
            {
                Size = propertyModel.Size,
                YardSize = propertyModel.YardSize,
                Floor = propertyModel.Floor,
                TotalFloors = propertyModel.TotalFloors,
                TotalBathRooms = propertyModel.TotalBathRooms,
                TotalBedRooms = propertyModel.TotalBedRooms,
                TotalGarages = propertyModel.TotalGarages,
                Year = propertyModel.Year,
                Price = propertyModel.Price,
                Description = propertyModel.Description,
                ExpirationDays = propertyModel.ExpirationDays,
                Option = propertyModel.Option,
                PropertyType = this.propertyTypeRepository.All().First(pt => pt.Id == propertyModel.TypeId),
            };

            var populatedPlace = this.populatedPlaceRepository.All().FirstOrDefault(p => p.Id == propertyModel.PopulatedPlaceId);

            property.PopulatedPlace = populatedPlace;

            var buildingType = propertyModel.BuildingTypes.First(x => x.IsChecked);

            var dbBuildingType = this.buildingTypeRepository.All().First(x => x.Id == buildingType.Id);

            property.BuildingType = dbBuildingType;

            property.ApplicationUser = user;

            await this.imageService.Add(propertyModel.Images, property);
            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public T GetById<T>(int id)
            => this.propertyRepository
                 .All()
                 .Where(p => p.Id == id)
                 .To<T>()
                 .FirstOrDefault();

        public T GetById<T>(int id, string userId)
            => this.propertyRepository
                 .All()
                 .Where(p => p.Id == id && p.ApplicationUserId == userId)
                 .To<T>()
                 .FirstOrDefault();

        public IEnumerable<PropertyViewModel> GetTopNewest(int count)
            => this.propertyRepository
                 .AllAsNoTracking()
                 .OrderByDescending(p => p.Id)
                 .Take(count)
                 .To<PropertyViewModel>()
                 .ToArray();

        public IEnumerable<PropertyViewModel> GetTopMostExpensive(int count)
            => this.propertyRepository
                 .AllAsNoTracking()
                 .OrderByDescending(p => p.Price)
                 .Take(count)
                 .To<PropertyViewModel>()
                 .ToArray();

        public int GetAllCount()
            => this.propertyRepository
                  .AllAsNoTracking()
                  .Count();

        public IEnumerable<PropertyViewModel> GetAllByOptionId(int optionId)
        {
            var result = optionId switch
            {
                (int)OptionType.NewToOld => this.propertyRepository.All().AsNoTracking().OrderByDescending(p => p.Id),
                (int)OptionType.OldToNew => this.propertyRepository.All().AsNoTracking().OrderBy(p => p.Id),
                (int)OptionType.ForSale => this.propertyRepository.All().AsNoTracking().Where(p => p.Option == PropertyOption.Sale).OrderByDescending(p => p.Id),
                (int)OptionType.ForRent => this.propertyRepository.All().AsNoTracking().Where(p => p.Option == PropertyOption.Rent).OrderByDescending(p => p.Id),
                (int)OptionType.PriceDesc => this.propertyRepository.All().AsNoTracking().OrderByDescending(p => p.Price),
                (int)OptionType.PriceAsc => this.propertyRepository.All().AsNoTracking().OrderBy(p => p.Price),
                _ => this.propertyRepository.All().AsNoTracking().OrderByDescending(p => p.Id),
            };

            return result
                .To<PropertyViewModel>()
                .ToArray();
        }

        public async Task<IEnumerable<PropertyViewModel>> GetAllByUserId(string id)
            => await this.propertyRepository
                  .All()
                  .AsNoTracking()
                  .Where(p => p.ApplicationUserId == id)
                  .OrderByDescending(p => p.Id)
                  .To<PropertyViewModel>()
                  .ToListAsync();

        public async Task Edit(EditViewModel editModel)
        {
            var dbProperty = await this.propertyRepository.All().FirstAsync(p => p.Id == editModel.Id);

            dbProperty.Size = editModel.Size;
            dbProperty.YardSize = editModel.YardSize;
            dbProperty.Floor = editModel.Floor;
            dbProperty.Price = editModel.Price;
            dbProperty.ExpirationDays = editModel.ExpirationDays;
            dbProperty.Description = editModel.Description;
            dbProperty.TotalBedRooms = editModel.TotalBedRooms;
            dbProperty.TotalBathRooms = editModel.TotalBathRooms;
            dbProperty.TotalGarages = editModel.TotalGarages;
            dbProperty.Year = editModel.Year;
            dbProperty.Option = editModel.Option;

            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task<AddPropertyInputModel> SetCollectionsAsync(AddPropertyInputModel property)
        {
            property.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
            property.Locations = this.locationService.Get<LocationViewModel>();
            property.BuildingTypes = this.buildingTypeService.Get<BuildingTypeViewModel>();
            property.Conditions = await this.conditionService.GetAllAsync();
            property.Heatings = await this.heatingService.GetAllAsync();
            property.Details = await this.detailService.GetAllAsync();
            property.Equipments = await this.equipmentService.GetAllAsync();

            return property;
        }
    }
}
