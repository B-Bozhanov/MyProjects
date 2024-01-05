namespace RealEstate.Web.ViewModels.Account
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RealEstate.Services.TranslateService.Models;
    using RealEstate.Web.Infrastructure.CustomAttributes;

    public class LoginViewModel
    {
        [Required]
        //[DisplayNameCustom(nameof(LanguageBase.Username))]
        public string Username { get; init; }

        [Required]
        public string Password { get; init; }
    }
}
