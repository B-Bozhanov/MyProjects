namespace RealEstate.Services.Data.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hangfire;

    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using RealEstate.Data;
    using RealEstate.Data.Common.Repositories;
    using RealEstate.Data.Models;
    using RealEstate.Data.Repositories;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Data.Tests.Mocks;
    using RealEstate.Services.Interfaces;
    using RealEstate.Web.ViewModels.BuildingTypeModel;
    using RealEstate.Web.ViewModels.ContactModel;
    using RealEstate.Web.ViewModels.Property;

    using Xunit;

    public class PropertyServiceTests
    {
        private readonly IPropertyService propertyService;
        private readonly IDeletableEntityRepository<Property> propertyRepository;

        public PropertyServiceTests()
        {
            this.propertyRepository = PropertyRepositoryMock.Instance;

            var userContactRepositoryMock = new Mock<IDeletableEntityRepository<UserContact>>();
            var conditionRepositoryMock = new Mock<IDeletableEntityRepository<Condition>>();
            var detailRepositoryMock = new Mock<IDeletableEntityRepository<Detail>>();
            var equipmentRepositoryMock = new Mock<IDeletableEntityRepository<Equipment>>();
            var heatingRepositoryMock = new Mock<IDeletableEntityRepository<Heating>>();

            var imageServiceMock = new Mock<IImageService>();
            var hangfireWrapperService = new Mock<IHangfireWrapperService>();
            var paginationService = new Mock<IPaginationService>();

            var propertyTypeRepositoryMock = PropertyTypeRepositoryMock.Instance;

            var buildingTypeRepositoryMock = BuildingTypeRepositoryMock.Instance;

            var populatedPlaceRepositoryMock = PopulatedPlaceRepositoryMock.Instance;

            this.propertyService = new PropertyService(propertyRepository, propertyTypeRepositoryMock,
                                                       buildingTypeRepositoryMock, userContactRepositoryMock.Object,
                                                       populatedPlaceRepositoryMock, conditionRepositoryMock.Object,
                                                       detailRepositoryMock.Object, equipmentRepositoryMock.Object,
                                                       heatingRepositoryMock.Object, imageServiceMock.Object,
                                                       hangfireWrapperService.Object, paginationService.Object);
        }

        [Fact]
        public async void AddAssyncShouldAddProperty()
        {
            //Arrange
            var propertyInputModel = new PropertyInputModel
            {
                Id = 1,
                PropertyTypeId = 2,
                BuildingTypes = new List<BuildingTypeViewModel>
                {
                    new BuildingTypeViewModel { Id = 1, Name = "BuildingType1", IsChecked = false},
                    new BuildingTypeViewModel { Id = 2, Name = "BuildingType2", IsChecked = true},
                    new BuildingTypeViewModel { Id = 3, Name = "BuildingType3",IsChecked = false},
                    new BuildingTypeViewModel { Id = 4, Name = "BuildingType4",IsChecked = false},
                },
                ExpirationDays = 30,
                PopulatedPlaceId = 2,
                ContactModel = new ContactModel
                {
                    Names = "Bozhan Bozhanov",
                    Email = "test@gmail.com",
                    PhoneNumber = "0896655070",
                },
            };
            var user = new ApplicationUser
            {
                FirstName = "Bozhan",
                LastName = "Bozhanov",
                UserName = "DareDeviL",
                Email = "test@gmail.com",
                PhoneNumber = "0896655707",
            };

            //Act

            await this.propertyService.AddAsync(propertyInputModel, user);
            
            //Assert

            Assert.True(propertyRepository.All().Count() == 1);
        }

        [Fact]
        public async Task AddAssyncShouldAddPropertyWithCorrectData()
        {
            //Arrange
            var propertyInputModel = new PropertyInputModel
            {
                Id = 1,
                PropertyTypeId = 2,
                Price = 129,
                Description = "test",
                Floor = 5,
                Option = PropertyOption.Sale,
                Size = 333,
                TotalBathRooms = 2,
                TotalBedRooms = 10,
                TotalGarages = 1,
                TotalFloors = 23,
                Year = 2020,
                YardSize = 500,
                BuildingTypes = new List<BuildingTypeViewModel>
                {
                    new BuildingTypeViewModel { Id = 1, Name = "BuildingType1", IsChecked = false},
                    new BuildingTypeViewModel { Id = 2, Name = "BuildingType2", IsChecked = true},
                    new BuildingTypeViewModel { Id = 3, Name = "BuildingType3",IsChecked = false},
                    new BuildingTypeViewModel { Id = 4, Name = "BuildingType4",IsChecked = false},
                },
                ExpirationDays = 30,
                PopulatedPlaceId = 2,
                ContactModel = new ContactModel
                {
                    Names = "Bozhan Bozhanov",
                    Email = "test@gmail.com",
                    PhoneNumber = "0896655070",
                },
            };
            var user = new ApplicationUser
            {
                FirstName = "Bozhan",
                LastName = "Bozhanov",
                UserName = "DareDeviL",
                Email = "test@gmail.com",
                PhoneNumber = "0896655707",
            };

            //Act
            await this.propertyService.AddAsync(propertyInputModel, user);
            var property = this.propertyRepository.All().FirstOrDefault();

            //Assert
            Assert.Equal(1, property.Id);
            Assert.Equal(2, property.BuildingTypeId);
            Assert.Equal(129, property.Price);
            Assert.Equal("test", property.Description);
            Assert.Equal(5, property.Floor);
            Assert.Equal(30, property.ExpirationDays);
            Assert.Equal(2, property.PopulatedPlaceId);
            Assert.Equal("Bozhan Bozhanov", property.UserContact.Names);
            Assert.Equal("test@gmail.com", property.UserContact.Email);
            Assert.Equal("0896655070", property.UserContact.PhoneNumber);
            Assert.Equal("Bozhan", property.ApplicationUser.FirstName);
            Assert.Equal("Bozhanov", property.ApplicationUser.LastName);
            Assert.Equal("DareDeviL", property.ApplicationUser.UserName);
            Assert.Equal("test@gmail.com", property.ApplicationUser.Email);
            Assert.Equal("0896655707", property.ApplicationUser.PhoneNumber);
            Assert.Equal(333, property.Size);
            Assert.Equal(2, property.TotalBathRooms);
            Assert.Equal(10, property.TotalBedRooms);
            Assert.Equal(1, property.TotalGarages);
            Assert.Equal(2020, property.Year);
            Assert.Equal(500, property.YardSize);
        }

        [Fact] 
        public async Task AddAssyncShouldThrowExceptionIfFloorIsNegative()
        {
            //Arrange
            var propertyInputModel = new PropertyInputModel
            {
                Id = 1,
                PropertyTypeId = 2,
                BuildingTypes = new List<BuildingTypeViewModel>
                {
                    new BuildingTypeViewModel { Id = 1, Name = "BuildingType1", IsChecked = false},
                    new BuildingTypeViewModel { Id = 2, Name = "BuildingType2", IsChecked = true},
                    new BuildingTypeViewModel { Id = 3, Name = "BuildingType3",IsChecked = false},
                    new BuildingTypeViewModel { Id = 4, Name = "BuildingType4",IsChecked = false},
                },
                ExpirationDays = 30,
                PopulatedPlaceId = 2,
                ContactModel = new ContactModel
                {
                    Names = "Bozhan Bozhanov",
                    Email = "test@gmail.com",
                    PhoneNumber = "0896655070",
                },
            };
            propertyInputModel.Floor = -5;
            var user = new ApplicationUser
            {
                FirstName = "Bozhan",
                LastName = "Bozhanov",
                UserName = "DareDeviL",
                Email = "test@gmail.com",
                PhoneNumber = "0896655707",
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async() => await this.propertyService.AddAsync(propertyInputModel, user));
            Assert.Equal(exception.Message, "The property floor can not be negative");
        }

        [Fact]
        public async Task AddAssyncShouldThrowExceptionIfFloorIsBiggerThanTotalFloors()
        {
            //Arrange
            var propertyInputModel = new PropertyInputModel
            {
                Id = 1,
                PropertyTypeId = 2,
                BuildingTypes = new List<BuildingTypeViewModel>
                {
                    new BuildingTypeViewModel { Id = 1, Name = "BuildingType1", IsChecked = false},
                    new BuildingTypeViewModel { Id = 2, Name = "BuildingType2", IsChecked = true},
                    new BuildingTypeViewModel { Id = 3, Name = "BuildingType3",IsChecked = false},
                    new BuildingTypeViewModel { Id = 4, Name = "BuildingType4",IsChecked = false},
                },
                ExpirationDays = 30,
                PopulatedPlaceId = 2,
                ContactModel = new ContactModel
                {
                    Names = "Bozhan Bozhanov",
                    Email = "test@gmail.com",
                    PhoneNumber = "0896655070",
                },
            };
            propertyInputModel.Floor = 5;
            propertyInputModel.TotalFloors = 3;
            var user = new ApplicationUser
            {
                FirstName = "Bozhan",
                LastName = "Bozhanov",
                UserName = "DareDeviL",
                Email = "test@gmail.com",
                PhoneNumber = "0896655707",
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.propertyService.AddAsync(propertyInputModel, user));
            Assert.Equal(exception.Message, "Current floor can no be bigger than total floors");
        }

        [Fact]
        public async Task AddAssyncShouldThrowExceptionIfBuildingTypesIsNull()
        {
            //Arrange
            var propertyInputModel = new PropertyInputModel
            {
                Id = 1,
                PropertyTypeId = 2,
                ExpirationDays = 30,
                PopulatedPlaceId = 2,
                ContactModel = new ContactModel
                {
                    Names = "Bozhan Bozhanov",
                    Email = "test@gmail.com",
                    PhoneNumber = "0896655070",
                },
            };
            var user = new ApplicationUser
            {
                FirstName = "Bozhan",
                LastName = "Bozhanov",
                UserName = "DareDeviL",
                Email = "test@gmail.com",
                PhoneNumber = "0896655707",
            };

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await this.propertyService.AddAsync(propertyInputModel, user));
        }

        [Fact]
        public async Task EditAssyncShouldChangePropertyData()
        {
            //Arrange
            var propertyInputModel = new PropertyInputModel
            {
                Id = 1,
                PropertyTypeId = 2,
                Price = 129,
                Description = "test",
                Floor = 5,
                Option = PropertyOption.Sale,
                Size = 333,
                TotalBathRooms = 2,
                TotalBedRooms = 10,
                TotalGarages = 1,
                TotalFloors = 23,
                Year = 2020,
                YardSize = 500,
                BuildingTypes = new List<BuildingTypeViewModel>
                {
                    new BuildingTypeViewModel { Id = 1, Name = "BuildingType1", IsChecked = false},
                    new BuildingTypeViewModel { Id = 2, Name = "BuildingType2", IsChecked = true},
                    new BuildingTypeViewModel { Id = 3, Name = "BuildingType3",IsChecked = false},
                    new BuildingTypeViewModel { Id = 4, Name = "BuildingType4",IsChecked = false},
                },
                ExpirationDays = 30,
                PopulatedPlaceId = 2,
                ContactModel = new ContactModel
                {
                    Names = "Bozhan Bozhanov",
                    Email = "test@gmail.com",
                    PhoneNumber = "0896655070",
                },
            };
            var user = new ApplicationUser
            {
                FirstName = "Bozhan",
                LastName = "Bozhanov",
                UserName = "DareDeviL",
                Email = "test@gmail.com",
                PhoneNumber = "0896655707",
            };

            var propertyEditViewModel = new PropertyEditViewModel
            {
                Id = 1,
                PropertyTypeId = 4,
                Price = 300,
                Description = "test2",
                Floor = 6,
                Option = PropertyOption.Rent,
                Size = 300,
                TotalBathRooms = 1,
                TotalBedRooms = 1,
                TotalGarages = 1,
                TotalFloors = 21,
                Year = 2021,
                YardSize = 501,
                BuildingTypes = new List<BuildingTypeViewModel>
                {
                    new BuildingTypeViewModel { Id = 1, Name = "BuildingType1", IsChecked = false},
                    new BuildingTypeViewModel { Id = 2, Name = "BuildingType2", IsChecked = false},
                    new BuildingTypeViewModel { Id = 3, Name = "BuildingType3",IsChecked = false},
                    new BuildingTypeViewModel { Id = 4, Name = "BuildingType4",IsChecked = true},
                },
                ExpirationDays = 31,
                PopulatedPlaceId = 4,
            };

            //Act
            await this.propertyService.AddAsync(propertyInputModel, user);
            await this.propertyService.EditAsync(propertyEditViewModel);

            var property = await this.propertyRepository.All().FirstAsync(x => x.Id == propertyInputModel.Id);

            //Assert
            Assert.Equal(1, (int)property.Option);
            Assert.Equal(4, property.PropertyTypeId);
            Assert.Equal(300, property.Price);
            Assert.Equal("test2", property.Description);
            Assert.Equal(6, property.Floor);
            Assert.Equal(300, property.Size);
            Assert.Equal(1, property.TotalBathRooms);
            Assert.Equal(1, property.TotalBedRooms);
            Assert.Equal(1, property.TotalGarages);
            Assert.Equal(21, property.TotalFloors);
            Assert.Equal(2021, property.Year);
            Assert.Equal(501, property.YardSize);
            Assert.Equal(4, property.BuildingTypeId);
            Assert.Equal(61, property.ExpirationDays);
            Assert.Equal(4, property.PopulatedPlaceId);
        }

        [Fact]
        public async Task EditAssyncShouldThrowExceptionIfPropertyNotFound()
        {
            //Arrange
            var propertyEditViewModel = new PropertyEditViewModel
            {
                Id = 1,
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await this.propertyService.EditAsync(propertyEditViewModel));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        public async Task GetAllActiveCountShouldReturnCorrectCount(int count)
        {
            await this.AddProperty(count);
            var result = this.propertyService.GetAllActiveCount();

            Assert.True(count == result);
        }

        private async Task AddProperty(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                var buildingTypes = new List<BuildingTypeViewModel>();

                for (int j = 1; j <= 5; j++)
                {
                    var buildingType = new BuildingTypeViewModel { Id = j, Name = $"BuildingType{j}" };

                    if (j == 1)
                    {
                        buildingType.IsChecked = true;
                    }

                    buildingTypes.Add(buildingType);
                }
                var propertyInputModel = new PropertyInputModel
                {
                    PropertyTypeId = i,
                    BuildingTypes = buildingTypes,
                    ExpirationDays = 30,
                    PopulatedPlaceId = i,
                    ContactModel = new ContactModel
                    {
                        Names = $"Name{i}",
                        Email = $"{i}@gmail.com",
                        PhoneNumber = $"089665570{i}",
                    },
                };

                var user = new ApplicationUser
                {
                    FirstName = $"Name{i}",
                    LastName = $"LastName{i}",
                    UserName = $"Username{i}",
                    Email = $"{i}@gmail.com",
                    PhoneNumber = $"089665570{i}",
                };
                await this.propertyService.AddAsync(propertyInputModel, user);
            }
        }

        private PropertyInputModel GetPropertyModel()
        {
            var propertyInputModel = new PropertyInputModel
            {
                Id = 1,
                PropertyTypeId = 2,
                BuildingTypes = new List<BuildingTypeViewModel>
                {
                    new BuildingTypeViewModel { Id = 1, Name = "BuildingType1", IsChecked = false},
                    new BuildingTypeViewModel { Id = 2, Name = "BuildingType2", IsChecked = true},
                    new BuildingTypeViewModel { Id = 3, Name = "BuildingType3",IsChecked = false},
                    new BuildingTypeViewModel { Id = 4, Name = "BuildingType4",IsChecked = false},
                },
                ExpirationDays = 30,
                PopulatedPlaceId = 2,
                ContactModel = new ContactModel
                {
                    Names = "Bozhan Bozhanov",
                    Email = "test@gmail.com",
                    PhoneNumber = "0896655070",
                },
            };

            return propertyInputModel;
        }

        private ApplicationUser GetUser()
        {
            return new ApplicationUser
            {
                FirstName = "Bozhan",
                LastName = "Bozhanov",
                UserName = "DareDeviL",
                Email = "test@gmail.com",
                PhoneNumber = "0896655707",
            };
        }
    }
}
