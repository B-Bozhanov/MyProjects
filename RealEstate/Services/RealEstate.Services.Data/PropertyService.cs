namespace RealEstate.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Hangfire;
    using Hangfire.Server;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data;
    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.PropertyTypes;
    using RealEstate.Web.ViewModels.Search;

    using static RealEstate.Common.GlobalConstants.Properties;

    public class PropertyService : IPropertyService
    {
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<PropertyType> propertyTypeRepository;
        private readonly IDeletableEntityRepository<BuildingType> buildingTypeRepository;
        private readonly IDeletableEntityRepository<UserContact> userContactsRepository;
        private readonly IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository;
        private readonly IDeletableEntityRepository<Condition> conditionRepository;
        private readonly IDeletableEntityRepository<Detail> detailRepository;
        private readonly IDeletableEntityRepository<Equipment> equipmentRepository;
        private readonly IDeletableEntityRepository<Heating> heatingRepository;
        private readonly ILocationService locationService;
        private readonly IBuildingTypeService buildingTypeService;
        private readonly IPropertyTypeService propertyTypeService;
        private readonly IPopulatedPlaceService placeService;
        private readonly IConditionService conditionService;
        private readonly IHeatingService heatingService;
        private readonly IEquipmentService equipmentService;
        private readonly IDetailService detailService;
        private readonly IImageService imageService;
        private readonly IBackgroundJobClient backgroundJobClient;

        private string jobId;

        public PropertyService(
              IDeletableEntityRepository<Property> propertyRepository,
              IDeletableEntityRepository<PropertyType> propertyTypeRepository,
              IDeletableEntityRepository<BuildingType> buildingTypeRepository,
              IDeletableEntityRepository<UserContact> userContactsRepository,
              IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository,
              IDeletableEntityRepository<Condition> conditionRepository,
              IDeletableEntityRepository<Detail> detailRepository,
              IDeletableEntityRepository<Equipment> equipmentRepository,
              IDeletableEntityRepository<Heating> heatingRepository,
              ILocationService locationService,
              IBuildingTypeService buildingTypeService,
              IPropertyTypeService propertyTypeService,
              IPopulatedPlaceService placeService,
              IConditionService conditionService,
              IHeatingService heatingService,
              IEquipmentService equipmentService,
              IDetailService detailService,
              IImageService imageService,
              IBackgroundJobClient backgroundJobClient)
        {
            this.propertyRepository = propertyRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.buildingTypeRepository = buildingTypeRepository;
            this.userContactsRepository = userContactsRepository;
            this.populatedPlaceRepository = populatedPlaceRepository;
            this.conditionRepository = conditionRepository;
            this.detailRepository = detailRepository;
            this.equipmentRepository = equipmentRepository;
            this.heatingRepository = heatingRepository;
            this.locationService = locationService;
            this.buildingTypeService = buildingTypeService;
            this.propertyTypeService = propertyTypeService;
            this.placeService = placeService;
            this.conditionService = conditionService;
            this.heatingService = heatingService;
            this.equipmentService = equipmentService;
            this.detailService = detailService;
            this.imageService = imageService;
            this.backgroundJobClient = backgroundJobClient;
        }

        public async Task AddAsync(PropertyInputModel propertyModel, ApplicationUser user, [CallerMemberName] string import = null!)
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
                PropertyType = this.propertyTypeRepository.All().First(pt => pt.Id == propertyModel.PropertyTypeId),
            };

            var populatedPlace = this.populatedPlaceRepository.All().FirstOrDefault(p => p.Id == propertyModel.PopulatedPlaceId);

            property.PopulatedPlace = populatedPlace;

            // TODO: Nullable building type.
            var buildingType = propertyModel.BuildingTypes.FirstOrDefault(x => x.IsChecked);

            if (buildingType != null)
            {
                property.BuildingType = this.buildingTypeRepository.All().First(x => x.Id == buildingType.Id);
            }

            property.ApplicationUser = user;
            property.UserContact = new UserContact
            {
                Names = propertyModel.ContactModel.Names,
                Email = propertyModel.ContactModel.Email,
                PhoneNumber = propertyModel.ContactModel.PhoneNumber,
            };

            property = await this.AddMoreDetailsAsync(propertyModel, property);
           
            await this.imageService.AddAsync(propertyModel.Images, property);
            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();

            this.backgroundJobClient.Schedule(() => this.AutoRemoveById(property.Id, null), TimeSpan.FromMinutes(property.ExpirationDays));
            RecurringJob.AddOrUpdate($"{property.Id}", () => this.ExpirationDaysDecreeser(property.Id, null), Cron.Minutely);
        }

        public async Task ExpirationDaysDecreeser(int propertyId, PerformContext performContext)
        {
            var property = await this.GetById(propertyId);
            if (property.ExpirationDays <= 0)
            {
                var jobId = performContext.BackgroundJob.Id;
                this.backgroundJobClient.Delete(jobId, null);
                return;
            }
            property.ExpirationDays--;
            this.propertyRepository.Update(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task Edit(PropertyEditViewModel editModel)
        {
            var property = await this.propertyRepository.All().FirstAsync(p => p.Id == editModel.Id);

            property.Size = editModel.Size;
            property.YardSize = editModel.YardSize;
            property.Floor = editModel.Floor;
            property.Price = editModel.Price;
            property.ExpirationDays += editModel.ExpirationDays;
            property.IsExpirationDaysModified = editModel.IsExpirationDaysModified;
            property.Description = editModel.Description;
            property.TotalBedRooms = editModel.TotalBedRooms;
            property.TotalBathRooms = editModel.TotalBathRooms;
            property.TotalGarages = editModel.TotalGarages;
            property.Year = editModel.Year;
            property.Option = editModel.Option;
            property.PopulatedPlaceId = editModel.PopulatedPlaceId;
            property.PropertyTypeId = editModel.PropertyTypeId; 

            var buildingType = editModel.BuildingTypes.FirstOrDefault(bt => bt.IsChecked);

            if (buildingType != null)
            {
                property.BuildingTypeId = buildingType.Id;
            }

            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
            => await this.propertyRepository
                 .All()
                 .Where(p => p.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();

        public async Task<T> GetByIdAsync<T>(int id, string userId)
            => await this.propertyRepository
                 .All()
                 .Where(p => p.Id == id && p.ApplicationUserId == userId)
                 .To<T>()
                 .FirstOrDefaultAsync();

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

        public async Task<IEnumerable<PropertyViewModel>> GetPaginationByUserId(string id, int page)
            => await this.propertyRepository
                  .AllWithDeleted()
                  .AsNoTracking()
                  .Where(p => p.ApplicationUserId == id)
                  .OrderByDescending(p => !p.IsDeleted)
                  .ThenByDescending(p => p.Id)
                  .Skip((page - 1) * PropertiesPerPage)
                  .Take(PropertiesPerPage)
                  .To<PropertyViewModel>()
                  .ToListAsync();

        public async Task<PropertyInputModel> SetCollectionsAsync(PropertyInputModel property)
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

        private async Task<Property> AddMoreDetailsAsync(PropertyInputModel propertyModel, Property property)
        {
            foreach (var condition in propertyModel.Conditions)
            {
                if (condition.IsChecked)
                {
                    var dbCondition = await this.conditionRepository.All().FirstAsync(c => c.Id == condition.Id);

                    property.Conditions.Add(dbCondition);
                }
            }

            foreach (var equipment in propertyModel.Equipments)
            {
                if (equipment.IsChecked)
                {
                    var dbEquipment = await this.equipmentRepository.All().FirstAsync(e => e.Id == equipment.Id);

                    property.Equipments.Add(dbEquipment);
                }
            }

            foreach (var detail in propertyModel.Details)
            {
                if (detail.IsChecked)
                {
                    var dbDetail = await this.detailRepository.All().FirstAsync(d => d.Id == detail.Id);

                    property.Details.Add(dbDetail);
                }
            }

            foreach (var heating in propertyModel.Heatings)
            {
                if (heating.IsChecked)
                {
                    var dbHeating = await this.heatingRepository.All().FirstAsync(h => h.Id == heating.Id);

                    property.Heatings.Add(dbHeating);
                }
            }

            return property;
        }

        public async Task<bool> IsUserProperty(int propertyId, string userId)
            => await this.propertyRepository
            .All()
            .AnyAsync(p => p.ApplicationUserId == userId);

        public async Task AutoRemoveById(int propertyId , PerformContext performContext)
        {
            var property = await this.GetById(propertyId);

            var jobId = performContext.BackgroundJob.Id;

            if (property.ExpirationDays <= 0)
            {
                this.propertyRepository.Delete(property);
                await this.propertyRepository.SaveChangesAsync();
                return;
            }

            this.backgroundJobClient.Reschedule(jobId, TimeSpan.FromMinutes(property.ExpirationDays));
        }

        //TODO: Remove this!!!

        private async Task<Property> GetById(int id)
            =>  await this.propertyRepository
            .All()
            .FirstAsync(p => p.Id == id);
    }
}
