namespace RealEstate.Web.Areas.Account.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;
    using RealEstate.Web.Controllers;
    using RealEstate.Web.ViewModels.Account;

    using static RealEstate.Common.GlobalConstants.Account.ErrorMessages;

    [Area("Account")]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginViewModel> logger;
        private readonly IAccountService accountService;
        private readonly IPropertyService propertyService;
        private readonly IPaginationService paginationService;
        private Cookie cookie;
        private CookieOptions cookieOptions;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginViewModel> logger,
            IAccountService accountService,
            IPropertyService propertyService,
            IPaginationService paginationService)
        {
            this.CreateReturnUrlCookie("ReturnUrl");
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.logger = logger;
            this.accountService = accountService;
            this.propertyService = propertyService;
            this.paginationService = paginationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return this.View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(registerModel);
            }

            var result = await this.accountService.RegisterAsync(registerModel);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.View(registerModel);
            }

            return this.RedirectToAction(nameof(this.LoginAsync));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/Login")]
        public async Task<IActionResult> LoginAsync(string returnUrl = null)
        {
            var loginModel = new LoginViewModel();

            if (!string.IsNullOrEmpty(loginModel.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, loginModel.ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            loginModel.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            loginModel.ReturnUrl = returnUrl;

            return this.View(loginModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/Login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginModel, string? returnUrl)
        {
            returnUrl ??= Url.Content("/MyProperties/ActiveProperties");

            loginModel.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                var result = await this.accountService.LoginAsync(loginModel);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User logged in.");
                    return Redirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    //TODO:
                    return RedirectToPage("/");
                }
                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return Redirect("/Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return this.View(loginModel);
                }
            }

            return this.View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await this.accountService.SignOutAsync();
            returnUrl ??= "/Login";
            return this.Redirect(returnUrl);
        }

        private void CreateReturnUrlCookie(string name)
        {
            this.cookie = new Cookie
            {
                Path = "/",
                Name = name,
                Version = 1,
            };

            this.cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddSeconds(30),
                Secure = true,
                Path = "/",
            };
        }

        private void SetCookie(string cookieName, string cookieValue, CookieOptions cookieOptions)
        {
            this.HttpContext.Response.Cookies.Append(cookieName, cookieValue, cookieOptions);
        }
    }
}

