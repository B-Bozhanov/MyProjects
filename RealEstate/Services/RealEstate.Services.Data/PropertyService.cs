namespace RealEstate.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire;
    using Hangfire.Server;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.Search;

    using static RealEstate.Common.GlobalConstants;

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
        private readonly IPropertySearchService propertySearchService;
        private readonly IPropertyGetService propertyGetService;
        private readonly IImageService imageService;
        private readonly IHangfireWrapperService hangfireWrapperService;
        private readonly IPaginationService paginationService;

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
              IPropertySearchService searchService,
              IPropertyGetService propertyGetService,
              IImageService imageService,
              IHangfireWrapperService hangfireWrapperService)
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
            this.propertySearchService = searchService;
            this.propertyGetService = propertyGetService;
            this.imageService = imageService;
            this.hangfireWrapperService = hangfireWrapperService;
        }

        public async Task AddAsync(PropertyInputModel propertyModel, string userId)
        {
            if (propertyModel.Floor < 0)
            {
                throw new InvalidOperationException("The property floor can not be negative");
            }
            if (propertyModel.Floor > propertyModel.TotalFloors)
            {
                throw new InvalidOperationException("Current floor can no be bigger than total floors");
            }
            if (propertyModel.BuildingTypes == null)
            {
                throw new ArgumentNullException("Building types is required");
            }

            var property = new Property()
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
                Option = (PropertyOption)propertyModel.Option,
            };
            property.PropertyType = this.propertyTypeRepository.All().FirstOrDefault(pt => pt.Id == propertyModel.PropertyTypeId);

            var populatedPlace = this.populatedPlaceRepository.All().FirstOrDefault(p => p.Id == propertyModel.PopulatedPlaceId);

            property.PopulatedPlace = populatedPlace;
            // TODO: Nullable building type.
            var buildingType = propertyModel.BuildingTypes.FirstOrDefault(x => x.IsChecked);

            if (buildingType != null)
            {
                property.BuildingType = this.buildingTypeRepository.All().FirstOrDefault(x => x.Id == buildingType.Id);
            }

            property.ApplicationUserId = userId;
            property.UserContact = new UserContact
            {
                Names = propertyModel.ContactModel.Names,
                Email = propertyModel.ContactModel.Email,
                PhoneNumber = propertyModel.ContactModel.PhoneNumber,
            };

            property = await this.AddMoreDetailsAsync(propertyModel, property);

            await this.imageService.AddAsync(propertyModel.Images, property, Images.SaveToLocalDrive);
            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();

            this.hangfireWrapperService.BackgroundJobClient.Schedule(() => this.RemoveByIdAsync(property.Id, null), TimeSpan.FromDays(property.ExpirationDays));
            this.hangfireWrapperService.RecurringJobManager.AddOrUpdate($"{property.Id}", () => this.ExpirationDaysDecreeser(property.Id, null), Cron.Daily);
        }

        public async Task RemoveByIdAsync(int propertyId, PerformContext performContext)
        {
            var property = await this.propertyGetService.GetByIdAsync(propertyId);

            if (property == null)
            {
                return;
            }

            var jobId = performContext.BackgroundJob.Id;

            if (property.ExpirationDays <= 0)
            {
                property.IsExpired = true;
                this.propertyRepository.Update(property);
                await this.propertyRepository.SaveChangesAsync();
                return;
            }

            this.hangfireWrapperService.BackgroundJobClient.Reschedule(jobId, TimeSpan.FromDays(property.ExpirationDays));
        }

        public async Task EditAsync(PropertyEditViewModel editModel)
        {
            var property = await this.propertyRepository.AllWithDeleted().FirstOrDefaultAsync(p => p.Id == editModel.Id);

            if (property == null)
            {
                throw new InvalidOperationException();
            }

            property.Size = editModel.Size;
            property.YardSize = editModel.YardSize;
            property.Floor = editModel.Floor;
            property.Price = editModel.Price;
            property.Description = editModel.Description;
            property.TotalBedRooms = editModel.TotalBedRooms;
            property.TotalBathRooms = editModel.TotalBathRooms;
            property.TotalFloors = editModel.TotalFloors;
            property.TotalGarages = editModel.TotalGarages;
            property.Year = editModel.Year;
            property.Option = (PropertyOption)editModel.Option;
            property.PopulatedPlaceId = editModel.PopulatedPlaceId;
            property.PropertyTypeId = editModel.PropertyTypeId;

            if (editModel.BuildingTypes != null)
            {
                var buildingType = editModel.BuildingTypes.FirstOrDefault(bt => bt.IsChecked);

                if (buildingType != null)
                {
                    property.BuildingTypeId = buildingType.Id;
                }
            }

            if (editModel.ExpirationDays > property.ExpirationDays)
            {
                property.ExpirationDays += editModel.ExpirationDays;

                if (property.IsExpired)
                {
                    property.IsExpired = false;
                    this.hangfireWrapperService.BackgroundJobClient.Schedule(() => this.RemoveByIdAsync(property.Id, null), TimeSpan.FromDays(property.ExpirationDays));
                    this.hangfireWrapperService.RecurringJobManager.AddOrUpdate($"{property.Id}", () => this.ExpirationDaysDecreeser(property.Id, null), Cron.Daily);
                }
            }

            this.propertyRepository.Update(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task ExpirationDaysDecreeser(int propertyId, PerformContext performContext)
        {
            var property = await this.propertyGetService.GetByIdAsync(propertyId);

            if (property == null || property.ExpirationDays <= 0)
            {
                var jobId = performContext.BackgroundJob.Id;
                this.hangfireWrapperService.RecurringJobManager.RemoveIfExists(jobId);
                performContext.Connection.Dispose();
                return;
            }

            property.ExpirationDays--;
            this.propertyRepository.Update(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task<bool> IsAnyExpiredProperties(string userId)
            => await this.propertyRepository
            .All()
            .AnyAsync(p => p.IsExpired);

        public async Task<bool> IsUserPropertyAsync(int propertyId, string userId)
            => await this.propertyRepository
            .AllWithDeleted()
            .AnyAsync(p => p.ApplicationUserId == userId);

        public async Task<bool> RemoveByIdAsync(int id)
        {
            var property = await this.propertyGetService.GetByIdAsync(id);

            if (property == null)
            {
                return false;
            }
            if (property.IsExpired)
            {
                this.propertyRepository.Delete(property);
            }

            property.ExpirationDays = default;
            property.IsExpired = true;
            await this.propertyRepository.SaveChangesAsync();
            return true;
        }

        private async Task<Property> AddMoreDetailsAsync(PropertyInputModel propertyModel, Property property)
        {
            if (propertyModel.Conditions != null)
            {
                foreach (var condition in propertyModel.Conditions)
                {
                    if (condition.IsChecked)
                    {
                        var dbCondition = await this.conditionRepository.All().FirstAsync(c => c.Id == condition.Id);

                        property.Conditions.Add(dbCondition);
                    }
                }
            }

            if (propertyModel.Equipments != null)
            {
                foreach (var equipment in propertyModel.Equipments)
                {
                    if (equipment.IsChecked)
                    {
                        var dbEquipment = await this.equipmentRepository.All().FirstAsync(e => e.Id == equipment.Id);

                        property.Equipments.Add(dbEquipment);
                    }
                }
            }

            if (propertyModel.Details != null)
            {
                foreach (var detail in propertyModel.Details)
                {
                    if (detail.IsChecked)
                    {
                        var dbDetail = await this.detailRepository.All().FirstAsync(d => d.Id == detail.Id);

                        property.Details.Add(dbDetail);
                    }
                }
            }

            if (propertyModel.Heatings != null)
            {
                foreach (var heating in propertyModel.Heatings)
                {
                    if (heating.IsChecked)
                    {
                        var dbHeating = await this.heatingRepository.All().FirstAsync(h => h.Id == heating.Id);

                        property.Heatings.Add(dbHeating);
                    }
                }
            }

            return property;
        }

        public async Task<IEnumerable<PropertyViewModel>> SearchAsync(SearchInputModel searchModel)
                 => await this.propertySearchService.SearchAsync<PropertyViewModel>(searchModel);

        public Dictionary<string, List<string>> PropertyValidator(PropertyInputModel property)
        {
            var errors = new Dictionary<string, List<string>>();

            if (property.BuildingTypes.Where(b => b.IsChecked).Count() > 1)
            {
                if (!errors.ContainsKey("AddPropertyErrors"))
                {
                    errors["AddPropertyErrors"] = new List<string>();
                }

                errors["AddPropertyErrors"].Add("Canot check more than one building type!");
            }
            if (property.BuildingTypes.All(b => !b.IsChecked))
            {
                if (!errors.ContainsKey("AddPropertyErrors"))
                {
                    errors["AddPropertyErrors"] = new List<string>();
                }

                errors["AddPropertyErrors"].Add("Building type is required!");
            }

            return errors;
        }
    }
}
