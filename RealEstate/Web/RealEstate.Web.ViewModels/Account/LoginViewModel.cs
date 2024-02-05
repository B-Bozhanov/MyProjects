namespace RealEstate.Web.ViewModels.Account
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.TranslateService.Models;
    using RealEstate.Web.Infrastructure.CustomAttributes;

    public class LoginViewModel
    {
        [Required]
        //[DisplayNameCustom(nameof(LanguageBase.Username))]
        public string Username { get; init; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
