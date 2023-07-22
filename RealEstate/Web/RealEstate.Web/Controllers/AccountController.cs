namespace RealEstate.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
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

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAccountService accountService,
            IPropertyService propertyService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.accountService = accountService;
            this.propertyService = propertyService;
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
        public async Task<IActionResult> Login(LoginViewModel loginModel)
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

            return this.RedirectToHome();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.accountService.SignOutAsync();

            return this.RedirectToHome();
        }

        [HttpGet]
        public async Task<IActionResult> UserProperties(int page = 1)
        {
            var propertiesCount = this.propertyService.GetAllCount();
            var paginationModel = new PaginationModel(propertiesCount, page);
            var currentProperties = await this.propertyService.GetPaginationByUserId(this.UserId, paginationModel.CurrentPage);

            this.ViewBag.Pager = paginationModel;
            return this.View(currentProperties);
        }
    }
}
