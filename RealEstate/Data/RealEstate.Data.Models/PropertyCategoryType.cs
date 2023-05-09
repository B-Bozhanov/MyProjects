namespace RealEstate.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class PropertyCategoryType : BaseDeletableModel<int>
    {
        public PropertyCategoryType()
        {
            this.PropertyTypes = new HashSet<PropertyType>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<PropertyType> PropertyTypes { get; set; }
    }
}
