namespace RealEstate.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.ContactModel;
    using RealEstate.Web.ViewModels.Districts;
    using RealEstate.Web.ViewModels.Places;
    using RealEstate.Web.ViewModels.Property;

    public class PropertyService : IPropertyService
    {
        private readonly IImageService imageService;
        private readonly IDeletableEntityRepository<Region> regionRepository;
        private readonly IDeletableEntityRepository<District> districtRepository;
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<PropertyType> propertyTypeRepository;
        private readonly IDeletableEntityRepository<BuildingType> buildingTypeRepository;
        private readonly IDeletableEntityRepository<Image> imageRepository;
        private readonly IDeletableEntityRepository<UserContact> userContactsRepository;
        private readonly IImageService imageService1;

        public PropertyService(
              IDeletableEntityRepository<Region> regionRepository,
              IDeletableEntityRepository<District> districtRepository,
              IDeletableEntityRepository<Property> propertyRepository,
              IDeletableEntityRepository<PropertyType> propertyTypeRepository,
              IDeletableEntityRepository<BuildingType> buildingTypeRepository,
              IDeletableEntityRepository<Image> imageRepository,
              IDeletableEntityRepository<UserContact> userContactsRepository,
              IImageService imageService)
        {
            this.regionRepository = regionRepository;
            this.districtRepository = districtRepository;
            this.propertyRepository = propertyRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.buildingTypeRepository = buildingTypeRepository;
            this.imageRepository = imageRepository;
            this.userContactsRepository = userContactsRepository;
            this.imageService1 = imageService;
        }

        public void Add(AddPropertyModel propertyModel, [CallerMemberName] string import = null!)
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
            };

            var districtName = propertyModel.District;
            var placeName = propertyModel.PlaceName;

            if (import == "Import")
            {
                var districtParts = propertyModel.District!.Split(',', StringSplitOptions.RemoveEmptyEntries);
                placeName = districtParts[0];
                districtName = districtParts[1];
                propertyModel.ContactModel = new ContactModel { Names = "Bozhan" };
            }

            var place = this.regionRepository.All().FirstOrDefault(p => p.Name == placeName);
            place ??= new Region { Name = placeName };

            var dbDistrict = this.districtRepository.All().FirstOrDefault(d => d.Name == districtName);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = districtName };

                // dbDistrict.Place = place;
            }

            var propertyType = this.propertyTypeRepository.All().FirstOrDefault(pt => pt.Name == propertyModel.Type);
            propertyType ??= new PropertyType { Name = propertyModel.Type };

            BuildingType dbBuildingType;
            if (propertyModel.BuildingType != null)
            {
                dbBuildingType = this.buildingTypeRepository.All().FirstOrDefault(bt => bt.Name == propertyModel.BuildingType);
                dbBuildingType ??= new BuildingType { Name = propertyModel.BuildingType };
            }
            else
            {
                var selectedType = propertyModel.BuildingTypes.FirstOrDefault(x => x.IsChecked);
                dbBuildingType = this.buildingTypeRepository.All().FirstOrDefault(bt => bt.Name == selectedType.Name);
            }

            var dbUserContacts = this.userContactsRepository.All().FirstOrDefault(u => u.Names == propertyModel.ContactModel.Names);
            dbUserContacts ??= new UserContact
            {
                Names = propertyModel.ContactModel.Names,
                Email = propertyModel.ContactModel.Email,
                PhoneNumber = propertyModel.ContactModel.PhoneNumber,
            };

            property.District = dbDistrict;
            property.BuildingType = dbBuildingType;
            property.PropertyType = propertyType;
            property.UserContact = dbUserContacts;
            property.PublishedOn = DateTime.Now;

            var images = this.imageService.GetImages(propertyModel.Images!);

            foreach (var image in images)
            {
                property.Images!.Add(image);
            }

            this.propertyRepository.AddAsync(property);

            this.propertyRepository.SaveChangesAsync();
        }

        public IEnumerable<Property> GetProperties() => this.propertyRepository.All();

        public IList<BuildingTypeModel> GetBuildingsTypes() => this.buildingTypeRepository
            .All()
            .Select(bt => new BuildingTypeModel
            {
                Name = bt.Name,
                Id = bt.Id,
            })
            .OrderBy(bt => bt.Name)
            .ToList();

        public IEnumerable<DistrictModel> GetDistricts() => this.districtRepository
            .All()
             .Select(d => new DistrictModel
             {
                 Name = d.Name,
                 Id = d.Id,
             })
            .OrderBy(d => d.Name)
            .ToList();

        public IEnumerable<PlaceModel> GetPlaces() => this.regionRepository
            .All()
            .Select(p => new PlaceModel
            {
                Name = p.Name,
                Id = p.Id,
            })
            .OrderBy(p => p.Name)
            .ToList();

        public IEnumerable<PropertyTypeViewModel> GetPropertiesTypes() => this.propertyTypeRepository
            .All()
            .Select(pt => new PropertyTypeViewModel
            {
                Name = pt.Name,
                Id = pt.Id,
            })
            .OrderBy(pt => pt.Name)
            .ToList();

        public IEnumerable<PropertyViewModel> GetTop10NewestSells()
        {
            var properties = this.propertyRepository
                .All()
                .Where(p => p.Options == PropertyOption.Sale)
                .OrderByDescending(p => p.Id)
                .Take(10)
                .Select(p => new PropertyViewModel
                {
                    Prize = p.Price,
                    Type = this.propertyTypeRepository
                    .All()
                    .Where(t => t.Id == p.Id)
                    .Select(t => new PropertyTypeViewModel
                    {
                        Id = t.Id,
                        Name = t.Name,
                    })
                    .First(),
                    District = this.districtRepository
                    .All()
                    .Where(d => d.Id == p.DistrictId)
                    .Select(d => new DistrictModel
                    {
                        Name = d.Name,
                        Id = d.Id,
                    })
                    .First(),
                })
                .ToList();

            return properties;
        }
    }
}
