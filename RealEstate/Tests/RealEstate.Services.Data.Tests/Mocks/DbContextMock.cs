namespace RealEstate.Services.Data.Tests.Mocks
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data;

    internal static class DbContextMock
    {
        internal static ApplicationDbContext Instance
        {
            get
            {
                var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

                var dbContext = new ApplicationDbContext(dbOptions);

                return dbContext;
            }
        }
    }
}
