namespace RealEstate.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class BuildingType : BaseDeletableModel<int>
    {
        public BuildingType()
        {
            this.Properties = new HashSet<Property>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
