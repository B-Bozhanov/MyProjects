﻿namespace RealEstate.Models.DataModels
{
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        public Tag()
        {
            this.Properties = new HashSet<Property>();
        }

        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}