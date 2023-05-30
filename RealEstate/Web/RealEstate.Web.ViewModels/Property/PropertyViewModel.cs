namespace RealEstate.Web.ViewModels.Property
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using AutoMapper;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.Locations;
    using RealEstate.Web.ViewModels.PopulatedPlaces;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public class PropertyViewModel : IMapFrom<Property>, IHaveCustomMappings
    {
        public int Id { get; init; }

        public PopulatedPlaceViewModel PopulatedPlace { get; init; }

        public string PropertyTypeName { get; set; }

        public string Price { get; init; }

        public string Option { get; set; }

        public int Size { get; init; }

        public string ImageName { get; init; }

        public int TotalBedRooms { get; init; }

        public int TotalBathRooms { get; init; }

        public int TotalGarages { get; init; }

        public void CreateMappings(IProfileExpression configuration)
        {
            var cultureInfo = new CultureInfo("fr-Fr");

            configuration.CreateMap<Property, PropertyViewModel>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => string.Format(cultureInfo, "{0:C2}", src.Price)))
                .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.Images.FirstOrDefault().Name));
        }
    }
}
