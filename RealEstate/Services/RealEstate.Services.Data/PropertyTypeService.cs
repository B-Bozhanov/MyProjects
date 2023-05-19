namespace RealEstate.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Mapping;

    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IDeletableEntityRepository<PropertyType> propertyTypeRepository;

        public PropertyTypeService(IDeletableEntityRepository<PropertyType> propertyTypeRepository)
        {
            this.propertyTypeRepository = propertyTypeRepository;
        }

        public IEnumerable<T> Get<T>()
            => this.propertyTypeRepository
                .All()
                .OrderBy(pt => pt.Name)
                .To<T>()
                .ToList();
    }
}
