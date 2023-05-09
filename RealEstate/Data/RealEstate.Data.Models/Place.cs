namespace RealEstate.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class Place : BaseDeletableModel<int>
    {
        public Place()
        {
            this.Properties = new HashSet<Property>();
        }

        [Required]
        public string Name { get; set; }

        public int RegionId { get; set; }

        [Required]
        public virtual Region Region { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
