namespace RealEstate.Web.ViewModels.Property
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;
    using RealEstate.Web.ViewModels.PopulatedPlaces;

    public class PropertyViewModel : IMapFrom<Property>
    {
        public int Id { get; init; }

        public PopulatedPlaceViewModel PopulatedPlace { get; init; }

        public string PropertyTypeName { get; set; }

        public string Price { get; init; }

        public string Option { get; set; }

        public int Size { get; init; }

        public int TotalBedRooms { get; init; }

        public int TotalBathRooms { get; init; }

        public int TotalGarages { get; init; }

        public int ExpirationDays { get; init; }

        public DateTime CreatedOn { get; init; }

        public DateTime ModifiedOn { get; init; }

        // TODO: Math.Round or something for the days.
        public bool IsExpired
        {
            get
            {
                var expiredAfter = 0;

                if (this.IsExpirationDaysModified)
                {
                    expiredAfter = (int)(DateTime.UtcNow - this.ModifiedOn).TotalMinutes;
                }
                else
                {
                    expiredAfter = (int)(DateTime.UtcNow - this.CreatedOn).TotalMinutes;
                }

                this.ExpireAfter = this.ExpirationDays - expiredAfter;

                if (this.ExpireAfter <= 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsExpirationDaysModified { get; set; }

        public int ExpireAfter { get; private set; }

        // TODO: Move to GlobalConstants
        public string ExpireMessage { get; init; } = "Expired!";

        public ICollection<ImageViewModel> Images { get; init; }
    }
}
