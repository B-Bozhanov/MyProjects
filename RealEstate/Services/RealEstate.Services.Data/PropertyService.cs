namespace RealEstate.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.Property;

    public class PropertyService : IPropertyService
    {
        private readonly IDeletableEntityRepository<Location> locationRepository;
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<PropertyType> propertyTypeRepository;
        private readonly IDeletableEntityRepository<BuildingType> buildingTypeRepository;
        private readonly IDeletableEntityRepository<UserContact> userContactsRepository;
        private readonly IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository;
        private readonly IImageService imageService;

        public PropertyService(
              IDeletableEntityRepository<Location> locationRepository,
              IDeletableEntityRepository<Property> propertyRepository,
              IDeletableEntityRepository<PropertyType> propertyTypeRepository,
              IDeletableEntityRepository<BuildingType> buildingTypeRepository,
              IDeletableEntityRepository<Image> imageRepository,
              IDeletableEntityRepository<UserContact> userContactsRepository,
              IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository,
              IImageService imageService)
        {
            this.locationRepository = locationRepository;
            this.propertyRepository = propertyRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.buildingTypeRepository = buildingTypeRepository;
            this.userContactsRepository = userContactsRepository;
            this.populatedPlaceRepository = populatedPlaceRepository;
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

            if (propertyModel.Images != null)
            {
                await this.imageService.Save(propertyModel.Images, property);
            }

            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public IEnumerable<PopulatedPlaceViewModel> GetPopulatedPlaces() => this.populatedPlaceRepository
            .All()
             .Select(d => new PopulatedPlaceViewModel
             {
                 Name = d.Name,
                 Id = d.Id,
             })
            .OrderBy(d => d.Name)
            .ToList();

        public IEnumerable<LocationViewModel> GetLocations() => this.locationRepository
            .All()
            .OrderBy(p => p.Name)
            .To<LocationViewModel>()
            .ToList();

        public PropertyViewModel GetById(int id)
            => this.propertyRepository
                 .AllAsNoTracking()
                 .Where(p => p.Id == id)
                 .To<PropertyViewModel>()
                 .First();

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

        public int GetAllCount() => this.propertyRepository
                  .AllAsNoTracking()
                  .Count();

        public IEnumerable<PropertyViewModel> GetAll()
            => this.propertyRepository.All()
                  .AsNoTracking()
                  .OrderByDescending(p => p.Id)
                  .To<PropertyViewModel>()
                  .ToArray();
    }
}
