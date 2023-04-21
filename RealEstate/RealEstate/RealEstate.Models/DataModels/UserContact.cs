namespace RealEstate.Models.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserContact
    {
        public UserContact()
        {
            this.Properties = new HashSet<Property>();
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Names { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Property> Properties { get; set; }
    }
}
