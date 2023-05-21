namespace RealEstate.Data.Models
{
    using System.Collections.Generic;

    using RealEstate.Data.Common.Models;

    public class PopulatedPlace : BaseDeletableModel<int>
    {
        public PopulatedPlace()
        {
            this.Properties = new HashSet<Property>();
        }

        public string Name { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}