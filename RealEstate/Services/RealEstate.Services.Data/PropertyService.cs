namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using RealEstate.Data;
    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.Property;

    public class PropertyService : IPropertyService
    {
        private readonly IImageService imageService;
        private readonly IDeletableEntityRepository<Location> locationRepository;
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<PropertyType> propertyTypeRepository;
        private readonly IDeletableEntityRepository<BuildingType> buildingTypeRepository;
        private readonly IDeletableEntityRepository<Image> imageRepository;
        private readonly IDeletableEntityRepository<UserContact> userContactsRepository;
        private readonly IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IImageService imageService1;

        public PropertyService(
              IDeletableEntityRepository<Location> regionRepository,
              IDeletableEntityRepository<Property> propertyRepository,
              IDeletableEntityRepository<PropertyType> propertyTypeRepository,
              IDeletableEntityRepository<BuildingType> buildingTypeRepository,
              IDeletableEntityRepository<Image> imageRepository,
              IDeletableEntityRepository<UserContact> userContactsRepository,
              IDeletableEntityRepository<PopulatedPlace> populatedPlaceRepository,
              IImageService imageService)
        {
            this.locationRepository = regionRepository;
            this.propertyRepository = propertyRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.buildingTypeRepository = buildingTypeRepository;
            this.imageRepository = imageRepository;
            this.userContactsRepository = userContactsRepository;
            this.populatedPlaceRepository = populatedPlaceRepository;
        }

        public async Task Add(AddPropertyViewModel propertyModel, [CallerMemberName] string import = null!)
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
                Options = propertyModel.Option,
                PropertyType = this.propertyTypeRepository.All().First(pt => pt.Id == propertyModel.TypeId),
            };

            var populatedPlace = this.populatedPlaceRepository.All().FirstOrDefault(p => p.Id == propertyModel.PopulatedPlaceId);

            property.PopulatedPlace = populatedPlace;

            var buildingType = propertyModel.BuildingTypes.First(x => x.IsChecked);

            var dbBuildingType = this.buildingTypeRepository.All().First(x => x.Id == buildingType.Id);

            property.BuildingType = dbBuildingType;

            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public IEnumerable<Property> Get() => this.propertyRepository.All();

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
    }
}
