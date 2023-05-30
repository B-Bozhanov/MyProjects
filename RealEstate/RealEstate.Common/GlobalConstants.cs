namespace RealEstate.Common
{
    using System.Collections.Generic;

    public static class GlobalConstants
    {
        public const string SystemName = "RealEstate";

        public const string SeedDataPath = @$"../../Data/{SystemName}.Data/Seeding/DataToSeed/";

        public const string AdministratorRoleName = "Administrator";

        public const string Layout = "_Theme1";

        public static List<Administrator> GetAdministrators()
        {
            var administrators = new List<Administrator>
            {
                new Administrator
                {
                    UserName = "DareDeviL88",
                    Password = "123456",
                    Email = "bojanilkov88@gmail.com",
                },
                new Administrator
                {
                    UserName = "Ribkata",
                    Password = "123456789",
                    Email = "iliyan.bojanov@ka1.bg",
                },
            };

            return administrators;
        }

        public static class Images
        {
            public const string ImagePath = "/assets/img/Properties/";
        }

        public static class Properties
        {
            public const int TopNewest = 3;

            public const int TopMostExpensive = 4;
        }
    }
}
