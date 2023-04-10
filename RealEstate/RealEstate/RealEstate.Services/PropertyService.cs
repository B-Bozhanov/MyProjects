namespace RealEstate.Services
{
    using RealEstate.App.Data;
    using RealEstate.Models.DataModels;
    using RealEstate.Models.ImportViewModels;
    using RealEstate.Services.Interfaces;

    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext context;

        public PropertyService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(PropertyViewModel propertyModel)
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

            var districtParts = propertyModel.District!.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var placeName = districtParts[0];
            var districtName = districtParts[1];

            var place = context.Places.FirstOrDefault(p => p.Name == placeName);
            place ??= new Place { Name = placeName };

            var dbDistrict = context.Districts.FirstOrDefault(d => d.Name == districtName);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = districtName };
                dbDistrict.Place = place;
            }

            var propertyType = context.PropertyTypes.FirstOrDefault(pt => pt.Name == propertyModel.Type);
            propertyType ??= new PropertyType { Name = propertyModel.Type };

            var dbBuildingType = context.BuildingTypes.FirstOrDefault(bt => bt.Name == propertyModel.BuildingType);
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
           
            context.Properties.Add(property);
            context.SaveChanges();
        }
    }
}
