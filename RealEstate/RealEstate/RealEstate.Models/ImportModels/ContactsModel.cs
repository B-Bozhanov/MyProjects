namespace RealEstate.Models.ImportModels
{
    using System.ComponentModel.DataAnnotations;

    public class ContactsModel
    {
        [Required]
        public string Names { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
