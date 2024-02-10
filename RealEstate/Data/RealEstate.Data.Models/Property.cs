namespace RealEstate.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class Property : BaseDeletableModel<int>
    {
        public Property()
        {
            this.Tags = new HashSet<Tag>();
            this.Images = new HashSet<Image>();
            this.Details = new HashSet<Detail>();
            this.Conditions = new HashSet<Condition>();
            this.Equipments = new HashSet<Equipment>();
            this.Heatings = new HashSet<Heating>();

            this.ExpirationDays = 90; // By default
            this.Option = PropertyOption.Sale; // By default
        }

        public bool IsSelled { get; set; }

        public bool IsRented { get; set; }

        public int Size { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public decimal? Price { get; set; }

        public PropertyOption Option { get; set; }

        public int ExpirationDays { get; set; }

        public bool IsExpired { get; set; }

#nullable enable
        public string? Description { get; set; }

        public int? TotalBedRooms { get; set; }

        public int? TotalBathRooms { get; set; }

        public int? TotalGarages { get; set; }

        public int? Year { get; set; }

        [Required]
        public int PopulatedPlaceId { get; set; }

        [Required]
        public virtual PopulatedPlace PopulatedPlace { get; set; }

        public int LocationId { get; set; }

        public virtual Location  Location{ get; set; }

        [Required]
        public int PropertyTypeId { get; set; }

        [Required]
        public virtual PropertyType PropertyType { get; set; }

        public int? BuildingTypeId { get; set; }

        #nullable enable
        public virtual BuildingType? BuildingType { get; set; }

        [Required]
        public string UserContactId { get; set; }

        [Required]
        public virtual UserContact UserContact { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        [Required]
        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Detail> Details { get; set; }

        public virtual ICollection<Condition> Conditions { get; set; }

        public virtual ICollection<Heating> Heatings { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }

    }
}
