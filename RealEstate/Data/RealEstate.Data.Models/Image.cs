namespace RealEstate.Data.Models
{
    using System;

    using RealEstate.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public int PropertyId { get; set; }

        public virtual Property Property { get; set; }
    }
}
