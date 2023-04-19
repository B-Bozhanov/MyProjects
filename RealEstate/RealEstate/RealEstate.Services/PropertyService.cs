namespace RealEstate.Services
{
    using System.Runtime.CompilerServices;

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

        public PropertyService
            (
              IPlaceRepository placeRepository,
              IDistrictRepository districtRepository,
              IPropertyRepository propertyRepository,
              IPropertyTypeRepository propertyTypeRepository,
              IBuildingTypeRepository buildingTypeRepository,
              IImageRepository imageRepository
            )
        {
            this.placeRepository = placeRepository;
            this.districtRepository = districtRepository;
            this.propertyRepository = propertyRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.buildingTypeRepository = buildingTypeRepository;
            this.imageRepository = imageRepository;
        }

        public void Add(AddPropertyModel propertyModel, [CallerMemberName]string import = null!)
        {
            var property = new Property
            {
                Url = propertyModel.Url,
                Size = propertyModel.Size,
                YardSize = propertyModel.YardSize,
                Floor = propertyModel.Floor,
                TotalFloors = propertyModel.TotalFloors,
                Year = propertyModel.Year,
                Price = propertyModel.Price,
            };

            var districtName = propertyModel.District;
            var placeName = propertyModel.PlaceName;

            if (import == "Import")
            {
                var districtParts = propertyModel.District!.Split(',', StringSplitOptions.RemoveEmptyEntries);
                placeName = districtParts[0];
                districtName = districtParts[1];
            }
           

            var place = this.placeRepository.All().FirstOrDefault(p => p.Name == placeName);
            place ??= new Place { Name = placeName };

            var dbDistrict = this.districtRepository.All().FirstOrDefault(d => d.Name == districtName);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = districtName };
                dbDistrict.Place = place;
            }

            var propertyType = this.propertyTypeRepository.All().FirstOrDefault(pt => pt.Name == propertyModel.Type);
            propertyType ??= new PropertyType { Name = propertyModel.Type };

            var dbBuildingType = this.buildingTypeRepository.All().FirstOrDefault(bt => bt.Name == propertyModel.BuildingType);
            dbBuildingType ??= new BuildingType { Name = propertyModel.BuildingType };


            property.District = dbDistrict;
            property.PropertyType = propertyType;
            property.BuildingType = dbBuildingType;
            property.PublishedOn = DateTime.Now;

            if (propertyModel.Images != null)
            {
                foreach (var image in propertyModel.Images!)
                {
                    property.Images!.Add(image);
                }
            }

            this.propertyRepository.Add(property);

            //this.propertyRepository.SaveChanges();
        }

        public IEnumerable<Property> GetProperties() => this.propertyRepository.All();

        public IEnumerable<BuildingType> GetBuildingsTypes() => this.buildingTypeRepository.All();

        public IEnumerable<District> GetDistricts() => this.districtRepository.All();

        public IEnumerable<Place> GetPlaces() => this.placeRepository.All();

        public IEnumerable<PropertyType> GetPropertiesTypes() => this.propertyTypeRepository.All();

        public IEnumerable<Property> GetTop10Newest()
        {
            var properties = this.propertyRepository.All().OrderByDescending(p => p.Id).Take(10);


            foreach (var property in properties)
            {
                property.Images = this.imageRepository.GetByPropertId(property.Id);
            }

            return new List<Property>();
        }
    }
}
