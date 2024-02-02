namespace RealEstate.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;

    using Newtonsoft.Json.Linq;

    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;
    using RealEstate.Web.ViewModels.Account;
    using RealEstate.Web.ViewModels.Property;

    using static RealEstate.Common.GlobalConstants.Account.ErrorMessages;
    using static RealEstate.Common.GlobalConstants.Properties;

    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAccountService accountService;
        private readonly IPropertyService propertyService;
        private readonly IPaginationService paginationService;
        private Cookie cookie;
        private CookieOptions cookieOptions;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAccountService accountService,
            IPropertyService propertyService, 
            IPaginationService paginationService) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.accountService = accountService;
            this.propertyService = propertyService;
            this.paginationService = paginationService;
            this.CreateReturnUrlCookie("ReturnUrl");
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

            return this.RedirectToAction(nameof(this.Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return this.View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginModel, string? returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(loginModel);
            }

            var result = await this.accountService.LoginAsync(loginModel);

            if (result == null || !result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, InvalidLogin);

                return this.View(loginModel);
            }

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.ActiveProperties));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.accountService.SignOutAsync();

            return this.RedirectToHome();
        }

        [HttpGet]
        [Route("/Account/PropertiesAll")]
        public async Task<IActionResult> UserProperties(int page = 1)
        {
            var propertiesCount = this.propertyService.GetAllActiveUserPropertiesCount(this.UserId);
            var paginationModel = this.paginationService.CreatePagination(propertiesCount, PropertiesPerPage, page, this.ControllerName(nameof(AccountController)), nameof(this.UserProperties));
            var currentProperties = await this.propertyService.GetAllWithExpiredUserPropertiesPerPage(this.UserId, paginationModel.CurrentPage);

            this.ViewBag.Pager = paginationModel;
            return this.View(currentProperties);
        }

        [HttpGet]
        [Route("/Account/ActiveProperties")]
        public async Task<IActionResult> ActiveProperties(int page = 1)
        {
            var propertiesCount = this.propertyService.GetAllActiveUserPropertiesCount(this.UserId);
            var paginationModel = this.paginationService.CreatePagination(propertiesCount, PropertiesPerPage, page, this.ControllerName(nameof(AccountController)), nameof(this.ActiveProperties));
            var currentProperties = await this.propertyService.GetActiveUserPropertiesPerPageAsync(this.UserId, paginationModel.CurrentPage);

            this.ViewBag.Pager = paginationModel;
            this.ViewBag.IsFromExpired = false;
            this.cookie.Value = "/Account/ActiveProperties";

            this.SetCookie(this.cookie.Name, this.cookie.Value, this.cookieOptions);
            return this.View(nameof(this.UserProperties), currentProperties);
        }

        [HttpGet]
        [Route("/Account/ExpiredProperties")]
        public async Task<IActionResult> ExpiredProperties(int page = 1)
        {
            var propertiesCount = this.propertyService.GetAllExpiredUserPropertiesCount(this.UserId);
            var paginationModel = this.paginationService.CreatePagination(propertiesCount, PropertiesPerPage, page, this.ControllerName(nameof(AccountController)), nameof(this.ExpiredProperties));
            var currentProperties = await this.propertyService.GetExpiredUserPropertiesPerPageAsync(this.UserId, paginationModel.CurrentPage);

            this.ViewBag.Pager = paginationModel;
            this.ViewBag.IsFromExpired = true;
            this.cookie.Value = "/Account/ExpiredProperties";
            
            this.SetCookie(this.cookie.Name, this.cookie.Value, this.cookieOptions);

            return this.View(nameof(this.UserProperties), currentProperties);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserProperty(int propertyId)
        {
            var isRemoved = await this.propertyService.RemoveByIdAsync(propertyId);

            if (isRemoved)
            {
                return this.Json(new { data = true });
            }

            return null; //TODO: Redirect to correct page;
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
