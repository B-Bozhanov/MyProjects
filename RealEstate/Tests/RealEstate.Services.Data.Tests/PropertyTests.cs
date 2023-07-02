namespace RealEstate.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using RealEstate.Data;
    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;

    using Xunit;

    public class PropertyTests
    {
        [Fact]
        public void AddAsyncShouldAddTheCorectPropertyInDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "PropertyTestDb").Options;
            using var dbContext = new ApplicationDbContext(options);

            var user = new ApplicationUser();

            var propertyService = new Mock<IPropertyService>();
        }

        [Fact]
        public async Task GetAllCountShouldWorkCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "PropertyTestDb").Options;
            using var dbContext = new ApplicationDbContext(options);

            var propertyService = new Mock<IPropertyService>();
        }
    }
}
