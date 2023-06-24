namespace RealEstate.Data.Models
{
    using System.Collections.Generic;

    using RealEstate.Data.Common.Models;

    public class Detail : BaseDeletableModel<int>
    {
        public Detail()
        {
            this.Properties = new HashSet<Property>();
        }

        public string Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}