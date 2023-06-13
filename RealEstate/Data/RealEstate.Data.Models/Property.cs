namespace RealEstate.Data.Models
{
    using System.Collections.Generic;

    using RealEstate.Data.Common.Models;

    public class Property : BaseDeletableModel<int>
    {
        public Property()
        {
            this.Tags = new HashSet<Tag>();
            this.Images = new HashSet<Image>();

            this.ExpirationDays = 90; // By default
            this.Option = PropertyOption.Sale; // By default
        }

        public int Size { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public decimal? Price { get; set; }

        public PropertyOption Option { get; set; }

        public int ExpirationDays { get; set; }

        public string Description { get; set; }

        public int? TotalBedRooms { get; set; }

        public int? TotalBathRooms { get; set; }

        public int? TotalGarages { get; set; }

        public int PopulatedPlaceId { get; set; }

        public virtual PopulatedPlace PopulatedPlace { get; set; }

        public int Year { get; set; }

        public int PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }

        public int BuildingTypeId { get; set; }

        public virtual BuildingType BuildingType { get; set; }

        public string UserContactId { get; set; }

        public virtual UserContact UserContact { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
