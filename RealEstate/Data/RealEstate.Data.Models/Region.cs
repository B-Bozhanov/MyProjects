namespace RealEstate.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class Region : BaseDeletableModel<string>
    {
        public Region()
        {
            this.Places = new HashSet<Place>();
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DownTownId { get; set; }

        public virtual DownTown DownTown { get; set; }

        [Required]
        public virtual ICollection<Place> Places { get; set; }
    }
}
