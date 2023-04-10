﻿namespace RealEstate.Models.DataModels
{
    public class Property
    {
        public Property()
        {
            this.Tags = new HashSet<Tag>();
            this.Images = new HashSet<Image>();

            this.ExpirationDays = 90; // By default
            this.Options = PropertyOption.Sale; // By default
        }

        public int Id { get; set; }

        public string? Url { get; set; }

        public int Size { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public decimal? Price { get; set; }

        public PropertyOption Options { get; set; }

        public int ExpirationDays { get; set; }

        public DateTime PublishedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? Description { get; set; }

        public int DistrictId { get; set; }

        public virtual District? District { get; set; }

        public int Year { get; set; }

        public int PropertyTypeId { get; set; }

        public virtual PropertyType? PropertyType { get; set; }

        public int BuildingTypeId { get; set; }

        public virtual BuildingType? BuildingType { get; set; }

        public virtual ICollection<Tag>? Tags { get; set; }

        public virtual ICollection<Image>? Images { get; set; }
    }
}
