namespace RealEstate.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    using static RealEstate.Common.GlobalConstants.Account;

    public class RegisterViewModel
    {
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
        [Compare(nameof(this.Password))]
        public string ConfirmPassword { get; init; }

        [Required]
        public bool IsTermsClicked { get; set; }
    }
}
