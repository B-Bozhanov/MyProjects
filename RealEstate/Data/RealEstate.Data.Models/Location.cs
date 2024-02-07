namespace RealEstate.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class Location : BaseDeletableModel<int>
    {
        public Location()
        {
            this.PopulatedPlaces = new HashSet<PopulatedPlace>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual ICollection<PopulatedPlace> PopulatedPlaces { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
