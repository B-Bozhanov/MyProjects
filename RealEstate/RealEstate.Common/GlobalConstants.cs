﻿namespace RealEstate.Common
{
    using System;
    using System.Collections.Generic;
    using static System.Net.WebRequestMethods;

    public static class GlobalConstants
    {
        public const string SystemName = "RealEstate";

        public const string CurrentSystemLanguage = "Bulgarian";

        public const string SeedDataPath = @$"../../Data/{SystemName}.Data/Seeding/DataToSeed/";

        public const string AdministratorRoleName = "Administrator";

        public const string Layout = "_Theme1";


        // TODO: Move administrators data to json in data folder
        public static List<Administrator> GetAdministrators()
        {
            var administrators = new List<Administrator>
            {
                new Administrator
                {
                    FirstName = "Bozhan",
                    LastName = "Bozhanov",
                    UserName = "DareDeviL88",
                    Password = "123456",
                    Email = "bojanilkov88@gmail.com",
                },
                new Administrator
                {
                    FirstName = "Iliyan",
                    LastName = "Bozhanov",
                    UserName = "Ribkata",
                    Password = "123456789",
                    Email = "iliyan.bojanov@ka1.bg",
                },
            };

            return administrators;
        }

        public static class Images
        {
            public const string AgentImagesUrl = "/assets/img/Agents/";
            public const int Height = 1080;
            public const string PropertyImagesUrl = "/assets/img/Properties/";
            public const bool SaveToLocalDrive = false;
            public const int ImageMaxSize = 1024 * 1024 * 10; // 10MB:
            public const string UploadUrl = "https://api.imgbb.com/1/upload/";
            public const int Width = 1920;


            public static HashSet<string> GetSupportedExtensions()
            {
                var extensions = new HashSet<string>
                {
                    ".jpg",
                    ".jpeg",
                    ".bnp",
                    ".png",
                    ".gif",
                };

                return extensions;
            }
        }

        public static class Properties
        {
            public const int TopNewest = 5;

            public const int TopMostExpensive = 3;

            public const int MaxGarages = 20;
            public const int MaxBedRooms = 20;
            public const int MaxBathRooms = 20;
            public const int MaxFloors = 150;

            public static int MinYear = 1800;

            public const int DefaultStartPage = 1;
            public const int PropertiesPerPage = 16;
            //public const string YearErrorMessage = $"The must be in range {MinYear} and {DateTime.UtcNow.Year}";

            public static class ErrorMessages
            {
                public const string Test = "Test";
            }
        }

        public static class Account
        {
            public const int UsernameMaxLength = 20;
            public const int UsernameMinLength = 3;

            public static class ErrorMessages
            {
                public const string InvalidLogin = "Invalid Login!";
            }
        }
    }
}
