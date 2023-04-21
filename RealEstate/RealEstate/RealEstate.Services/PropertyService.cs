namespace RealEstate.Services
{
    using System.Runtime.CompilerServices;

    using Microsoft.AspNetCore.Http;

    using RealEstate.Data.Interfaces;
    using RealEstate.Models.DataModels;
    using RealEstate.Models.ImportModels;
    using RealEstate.Services.Interfaces;

    public class PropertyService : IPropertyService
    {
        private readonly IPlaceRepository placeRepository;
        private readonly IDistrictRepository districtRepository;
        private readonly IPropertyRepository propertyRepository;
        private readonly IPropertyTypeRepository propertyTypeRepository;
        private readonly IBuildingTypeRepository buildingTypeRepository;
        private readonly IImageRepository imageRepository;
        private readonly IUserContactsRepository userContactsRepository;

        public PropertyService
            (
              IPlaceRepository placeRepository,
              IDistrictRepository districtRepository,
              IPropertyRepository propertyRepository,
              IPropertyTypeRepository propertyTypeRepository,
              IBuildingTypeRepository buildingTypeRepository,
              IImageRepository imageRepository,
              IUserContactsRepository userContactsRepository
            )
        {
            this.placeRepository = placeRepository;
            this.districtRepository = districtRepository;
            this.propertyRepository = propertyRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.buildingTypeRepository = buildingTypeRepository;
            this.imageRepository = imageRepository;
            this.userContactsRepository = userContactsRepository;
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
            }


            var place = this.placeRepository.Get(p => p.Name == placeName).FirstOrDefault(); 
            place ??= new Place { Name = placeName };

            var dbDistrict = this.districtRepository.Get(d => d.Name == districtName).FirstOrDefault();
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = districtName };
                dbDistrict.Place = place;
            }

            var propertyType = this.propertyTypeRepository.Get(pt => pt.Name == propertyModel.Type).FirstOrDefault();
            propertyType ??= new PropertyType { Name = propertyModel.Type };

            var dbBuildingType = this.buildingTypeRepository.Get(bt => bt.Name == propertyModel.BuildingType).FirstOrDefault();
            dbBuildingType ??= new BuildingType { Name = propertyModel.BuildingType };


            var dbUserContacts = this.userContactsRepository.Get(u => u.Names == propertyModel.ContactsModel.Names).FirstOrDefault();
            dbUserContacts ??= new UserContact
            {
                Names = propertyModel.ContactsModel.Names,
                Email = propertyModel.ContactsModel.Email,
                PhoneNumber = propertyModel.ContactsModel.PhoneNumber,
            };

            property.District = dbDistrict;
            property.PropertyType = propertyType;
            property.BuildingType = dbBuildingType;
            property.UserContact = dbUserContacts;
            property.PublishedOn = DateTime.Now;

            var images = this.GetImages(propertyModel.Images!);

            foreach (var image in images)
            {
                property.Images!.Add(image);
            }

            this.propertyRepository.Add(property);

            //this.propertyRepository.SaveChanges();
        }

        public IEnumerable<Property> GetProperties() => this.propertyRepository.All();

        public IList<BuildingTypeModel> GetBuildingsTypes() => this.buildingTypeRepository
            .All()
            .Select(bt => new BuildingTypeModel
            {
                Name = bt.Name,
                Id = bt.Id
            })
            .OrderBy(bt => bt.Name)
            .ToList();

        public IEnumerable<DistrictsModel> GetDistricts() => this.districtRepository
            .All()
             .Select(d => new DistrictsModel
             {
                 Name = d.Name,
                 Id = d.Id
             })
            .OrderBy(d => d.Name)
            .ToList();

        public IEnumerable<PlacesModel> GetPlaces() => this.placeRepository
            .All()
            .Select(p => new PlacesModel
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

        public IEnumerable<Property> GetTop10Newest()
        {
            var properties = this.propertyRepository.All().OrderByDescending(p => p.Id).Take(10);


            foreach (var property in properties)
            {
                property.Images = this.imageRepository.GetByPropertId(property.Id);
            }

            return new List<Property>();
        }

        private ICollection<Image> GetImages(IFormFileCollection files)
        {
            var images = new List<Image>();

            if (files != null)
            {
                foreach (var file in files)
                {
                    var stream = new MemoryStream();
                    file.CopyTo(stream);

                    var image = new Image { Name = file.Name, Content = stream.ToArray() };

                    images.Add(image);
                }
            }

            return images;
        }
    }
}
