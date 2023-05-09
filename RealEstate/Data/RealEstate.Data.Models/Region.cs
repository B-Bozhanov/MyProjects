namespace RealEstate.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class Region : BaseDeletableModel<int>
    {
        public Region()
        {
            this.Places = new HashSet<Place>();
        }

        [Required]
        public string Name { get; set; }

        public int DownTownId { get; set; }

        public virtual DownTown DownTown { get; set; }

        [Required]
        public virtual ICollection<Place> Places { get; set; }
    }
}
