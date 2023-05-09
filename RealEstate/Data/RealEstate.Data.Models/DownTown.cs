namespace RealEstate.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class DownTown : BaseDeletableModel<int>
    {
        public DownTown()
        {
            this.Districts = new HashSet<District>();
        }

        [Required]
        public string Name { get; set; } = null!;

        public int RegionId { get; set; }

        [Required]
        public virtual Region Region { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
