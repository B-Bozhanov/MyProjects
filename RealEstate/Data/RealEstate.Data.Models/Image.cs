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

        public string DeleteUrl { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public long Size { get; set; }

        public int Expiration { get; set; }

        public string Extension { get; set; }

        [Required]
        public int PropertyId { get; set; }

        public virtual Property Property { get; set; }
    }
}
