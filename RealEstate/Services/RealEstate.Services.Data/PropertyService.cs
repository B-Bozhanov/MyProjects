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

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.Property;
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
        private readonly IImageService imageService;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly IHangfireWrapperService hangfireWrapper;

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
              IImageService imageService,
              IHangfireWrapperService hangfireWrapper)
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
            this.imageService = imageService;
            this.hangfireWrapper = hangfireWrapper;
        }

        public async Task AddAsync(PropertyInputModel propertyModel, ApplicationUser user, [CallerMemberName] string import = null!)
        {
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

            this.hangfireWrapper.BackgroundJobClient.Schedule(() => this.AutoRemoveById(property.Id, null), TimeSpan.FromMinutes(property.ExpirationDays));
            this.hangfireWrapper.RecurringJobManager.AddOrUpdate($"{property.Id}", () => this.ExpirationDaysDecreeser(property.Id, null), Cron.Minutely);
            //RecurringJob.AddOrUpdate($"{property.Id}", () => this.ExpirationDaysDecreeser(property.Id, null), Cron.Minutely);
        }

        public async Task AutoRemoveById(int propertyId, PerformContext performContext)
        {
            var property = await this.GetByIdAsync(propertyId);

            var jobId = performContext.BackgroundJob.Id;

            if (property.ExpirationDays <= 0)
            {
                property.IsExpired = true;
                this.propertyRepository.Update(property);
                await this.propertyRepository.SaveChangesAsync();
                return;
            }

            this.backgroundJobClient.Reschedule(jobId, TimeSpan.FromMinutes(property.ExpirationDays));
        }

        public async Task EditAsync(PropertyEditViewModel editModel)
        {
            var property = await this.propertyRepository.AllWithDeleted().FirstAsync(p => p.Id == editModel.Id);

            property.Size = editModel.Size;
            property.YardSize = editModel.YardSize;
            property.Floor = editModel.Floor;
            property.Price = editModel.Price;
            property.IsExpirationDaysModified = editModel.IsExpirationDaysModified;
            property.IsExpired = editModel.IsExpired;
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

            if (editModel.ExpirationDays > property.ExpirationDays)
            {
                property.ExpirationDays += editModel.ExpirationDays;

                if (property.IsExpired)
                {
                    property.IsExpired = false;
                    this.hangfireWrapper.BackgroundJobClient.Schedule(() => this.AutoRemoveById(property.Id, null), TimeSpan.FromMinutes(property.ExpirationDays));
                    this.hangfireWrapper.RecurringJobManager.AddOrUpdate($"{property.Id}", () => this.ExpirationDaysDecreeser(property.Id, null), Cron.Minutely);
                }
            }

            //if (property.ExpirationDays <= 0 )
            //{
            //    if (property.IsExpirationDaysModified)
            //    {
            //        property.IsExpired = false;

            //        this.hangfireWrapper.BackgroundJobClient.Schedule(() => this.AutoRemoveById(property.Id, null), TimeSpan.FromMinutes(property.ExpirationDays));
            //        this.hangfireWrapper.RecurringJobManager.AddOrUpdate($"{property.Id}", () => this.ExpirationDaysDecreeser(property.Id, null), Cron.Minutely);
            //        //RecurringJob.AddOrUpdate($"{property.Id}", () => this.ExpirationDaysDecreeser(property.Id, null), Cron.Minutely);
            //    }
            //}

            this.propertyRepository.Update(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task ExpirationDaysDecreeser(int propertyId, PerformContext performContext)
        {
            var property = await this.GetByIdAsync(propertyId);

            if (property.ExpirationDays <= 0)
            {
                var jobId = performContext.BackgroundJob.Id;
                this.hangfireWrapper.RecurringJobManager.RemoveIfExists(jobId);
                performContext.Connection.Dispose();
                return;
            }

            property.ExpirationDays--;
            this.propertyRepository.Update(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task<int> GetAllExpiredProperties()
        {
            return this.propertyRepository
                .All()
                .Where(p => p.IsDeleted)
                .Count();
        }

        public async Task<T> GetByIdAsync<T>(int id)
            => await this.propertyRepository
                 .All()
                 .Where(p => p.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();

        public async Task<T> GetByIdWithExpiredAsync<T>(int id, string userId)
            => await this.propertyRepository
                 .AllWithDeleted()
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
                  .All()
                  .ToArray()
                  .Length;

        public int GetAllActiveUserPropertiesCount(string userId)
            => this.propertyRepository
            .All()
            .Where(p => p.ApplicationUserId == userId && !p.IsExpired)
            .OrderByDescending(p => p.Id)
            .ToArray()
            .Length;

        public int GetAllExpiredUserPropertiesCount(string userId)
            => this.propertyRepository
            .All()
            .Where(p => p.ApplicationUserId == userId && p.IsExpired)
            .OrderByDescending(p => p.DeletedOn)
            .ToArray()
            .Length;

        public IEnumerable<PropertyViewModel> GetAllByOptionIdPerPage(int optionId, int page)
        {
            var result = optionId switch
            {
                (int)OptionType.NewToOld => this.propertyRepository.AllAsNoTracking().OrderByDescending(p => p.Id),
                (int)OptionType.OldToNew => this.propertyRepository.AllAsNoTracking().OrderBy(p => p.Id),
                (int)OptionType.ForSale => this.propertyRepository.AllAsNoTracking().Where(p => p.Option == PropertyOption.Sale).OrderByDescending(p => p.Id),
                (int)OptionType.ForRent => this.propertyRepository.AllAsNoTracking().Where(p => p.Option == PropertyOption.Rent).OrderByDescending(p => p.Id),
                (int)OptionType.PriceDesc => this.propertyRepository.AllAsNoTracking().OrderByDescending(p => p.Price),
                (int)OptionType.PriceAsc => this.propertyRepository.AllAsNoTracking().OrderBy(p => p.Price),
                _ => this.propertyRepository.All().AsNoTracking().OrderByDescending(p => p.Id),
            };

            return result
                .Skip((page - 1) * PropertiesPerPage)
                .Take(PropertiesPerPage)
                .To<PropertyViewModel>()
                .ToArray();
        }

        public async Task<IEnumerable<PropertyViewModel>> GetActiveUserPropertiesPerPageAsync(string id, int page)
        {
            var activeProperties = await this.propertyRepository
                 .All()
                 .Where(p => p.ApplicationUserId == id && !p.IsExpired)
                 .OrderByDescending(p => p.Id)
                 .To<PropertyViewModel>()
                 .ToListAsync();

            return this.Pager(activeProperties, page);
        }

        public async Task<IEnumerable<PropertyViewModel>> GetExpiredUserPropertiesPerPageAsync(string id, int page)
        {
            var expiredProperties = await this.propertyRepository
                 .All()
                 .Where(p => p.ApplicationUserId == id && p.IsExpired)
                 .OrderByDescending(p => p.DeletedOn)
                 .To<PropertyViewModel>()
                 .ToListAsync();

            return this.Pager(expiredProperties, page);
        }

        public async Task<bool> IsAnyExpiredProperties(string userId)
            => await this.propertyRepository
            .All()
            .AnyAsync(p => p.IsExpired);

        public async Task<bool> IsUserProperty(int propertyId, string userId)
            => await this.propertyRepository
            .All()
            .AnyAsync(p => p.ApplicationUserId == userId);

        public async Task RemoveByIdAsync(int id)
        {
            var property = await this.GetByIdAsync(id);
            this.propertyRepository.Delete(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        //TODO: Remove this!!!
        private async Task<Property> GetByIdAsync(int id)
            => await this.propertyRepository
            .All()
            .FirstAsync(p => p.Id == id);

        public async Task<IEnumerable<PropertyViewModel>> SearchAsync(SearchViewModel searchModel)
        {
            var result = new List<PropertyViewModel>();

            if (searchModel.KeyWord == null && searchModel.Type == null
                && searchModel.BathRooms == null && searchModel.BedRooms == null
                && searchModel.LocationId == null && searchModel.PopulatedPlaceId == null
                && searchModel.Garages == null && searchModel.MinPrice == null && searchModel.MaxPrice == null)
            {
                throw new InvalidOperationException("Select at least one criteria");
            }

            //if (searchModel.KeyWord != null && searchModel.Type == null
            //  && searchModel.BathRooms == null && searchModel.BedRooms == null
            //  && searchModel.LocationId == null && searchModel.PopulatedPlaceId == null
            //  && searchModel.Garages == null && searchModel.MinPrice == null && searchModel.MaxPrice == null)
            //{
            //    result = this.propertyRepository
            //        .AllAsNoTracking()
            //        .ToList()
            //        .Where(p => p.GetType()
            //              .GetProperties()
            //              .Any(x => x.GetValue().Contains(searchModel.KeyWord)))
            //        .Select(x => new PropertyViewModel
            //        {
            //            ExpirationDays = x.ExpirationDays,
            //            Id = x.Id,
            //            IsExpirationDaysModified = x.IsExpirationDaysModified,
            //            IsExpired = x.IsExpired,
            //            Option = x.Option.ToString(),
            //            PopulatedPlace = new PopulatedPlaceViewModel
            //            {
            //                Id = x.PopulatedPlace.Id,
            //                Location = new LocationViewModel
            //                {
            //                    Id = x.PopulatedPlace.LocationId,
            //                    Name = x.PopulatedPlace.Location.Name,
            //                },
            //                Name = x.PopulatedPlace.Name,
            //            },
            //            PropertyTypeName = x.PropertyType.Name,
            //            TotalBathRooms = x.TotalBathRooms,
            //            TotalBedRooms = x.TotalBedRooms,
            //            Size = x.Size,
            //            TotalGarages = x.TotalGarages,
            //            Price = x.Price.ToString(),

            //        }).ToList();
            //}


                return result;
        }

        private IEnumerable<PropertyViewModel> Pager(IEnumerable<PropertyViewModel> properties, int page)
            => properties
                .Skip((page - 1) * PropertiesPerPage)
                .Take(PropertiesPerPage)
                .ToList();

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

    }
}
