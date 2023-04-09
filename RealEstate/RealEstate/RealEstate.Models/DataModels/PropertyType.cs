namespace RealEstate.Models.DataModels
{
    using System.ComponentModel.DataAnnotations;

    public class PropertyType
    {
        public PropertyType()
        {
            this.Properties = new HashSet<Property>();
        }

        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}