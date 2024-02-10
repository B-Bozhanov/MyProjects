namespace RealEstate.Services.Data.Tests.Mocks
{
    using Moq;

    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.HangFireWrapper;
    using RealEstate.Services.Interfaces;

    internal class PropertyServiceMock
    {
        IDeletableEntityRepository<Property> propertyRepository;

        internal PropertyServiceMock(IDeletableEntityRepository<Property> propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        internal IPropertyService Instance
        {
            get
            {
                var buildingTypeRepositoryMock = BuildingTypeRepositoryMock.Instance;
                var conditionRepositoryMock = ConditionRepositoryMock.Instance;
                var detailRepositoryMock = DetailRepositoryMock.Instance;
                var equipmentRepositoryMock = EquipmentRepositoryMock.Instance;
                var heatingRepositoryMock = HeatingRepositoryMock.Instance;
                var populatedPlaceRepositoryMock = PopulatedPlaceRepositoryMock.Instance;
                var propertyTypeRepositoryMock = PropertyTypeRepositoryMock.Instance;

                var hangfireWrapperService = HangFireWrapperServiceMock.Instance;
                var imageServiceMock = ImageServiceMock.Instance;
                var paginationService = PaginationServiceMock.Instance;
                var userContactRepositoryMock = UserContactRepositoryMock.Instance;

                return new PropertyService(this.propertyRepository, propertyTypeRepositoryMock,
                                           buildingTypeRepositoryMock, userContactRepositoryMock,
                                           populatedPlaceRepositoryMock, conditionRepositoryMock,
                                           detailRepositoryMock, equipmentRepositoryMock,
                                           heatingRepositoryMock, new Mock<IPropertySearchService>().Object,new Mock<IPropertyGetService>().Object, imageServiceMock,
                                           hangfireWrapperService);
            }
        }
    }
}
