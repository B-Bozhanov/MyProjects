namespace RealEstate.Web.ViewModels.ContactModel
{
    using System.ComponentModel.DataAnnotations;

    public class ContactModel
    {
        [Required]
        public string Names { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "The Phone number field is required!")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
