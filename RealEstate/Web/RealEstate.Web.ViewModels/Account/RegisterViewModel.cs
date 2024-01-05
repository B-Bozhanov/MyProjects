namespace RealEstate.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    using static RealEstate.Common.GlobalConstants.Account;

    public class RegisterViewModel
    {
        public string AgencyName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(this.Password))]
        public string ConfirmPassword { get; init; }

        [Required]
        public bool IsTermsClicked { get; set; }

        public bool IsAgent { get; set; }
    }
}
