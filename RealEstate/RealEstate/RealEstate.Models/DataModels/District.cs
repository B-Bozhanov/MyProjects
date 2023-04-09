namespace RealEstate.Models.DataModels
{
    using System.ComponentModel.DataAnnotations;

    public class District
    {
        public District()
        {
            this.Properties = new HashSet<Property>();
        }

        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public int PlaceId { get; set; }

        public virtual Place? Place { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}