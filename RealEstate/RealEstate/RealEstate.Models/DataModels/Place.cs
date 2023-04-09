namespace RealEstate.Models.DataModels
{
    using System.ComponentModel.DataAnnotations;

    public class Place
    {
        public Place()
        {
            this.Districts = new HashSet<District>();
        }

        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}