namespace RealEstate.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Data.Common.Models;

    public class Image : BaseDeletableModel<Guid>
    {
        public Image()
        {
            this.Id = Guid.NewGuid();
        }

        [Required]
        public string Url { get; set; }

        [Required]
        public int PropertyId { get; set; }

        public virtual Property Property { get; set; }
    }
}
