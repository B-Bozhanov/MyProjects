namespace RealEstate.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class DownTown : BaseDeletableModel<string>
    {
        public DownTown()
        {
            this.Districts = new HashSet<District>();
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string RegionId { get; set; }

        public virtual Location Region { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
