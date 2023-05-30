namespace RealEstate.Data.Models
{
    using RealEstate.Data.Common.Models;

    public class Image : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public int PropertyId { get; set; }

        public virtual Property Property { get; set; }
    }
}
