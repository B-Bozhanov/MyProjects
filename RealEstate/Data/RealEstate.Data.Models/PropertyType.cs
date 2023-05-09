namespace RealEstate.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class PropertyType : BaseDeletableModel<int>
    {
        public PropertyType()
        {
            this.Properties = new HashSet<Property>();
        }

        public int PropertyCategoryTypeId { get; set; }

        [Required]
        public virtual PropertyCategoryType PropertyCategoryType { get; set; } = null!;

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
