namespace RealEstate.Web.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Interfaces;

    using static RealEstate.Common.GlobalConstants.Properties;

    public class MyPropertyController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IPaginationService paginationService;
        private CookieOptions cookieOptions;
        private Cookie cookie;

        public MyPropertyController(IPropertyService propertyService, IPaginationService paginationService)
        {
            this.propertyService = propertyService;
            this.paginationService = paginationService;
            this.CreateReturnUrlCookie("ReturnUrl");
        }

        [HttpGet]
        [Route("/MyProperties/PropertiesAll")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var propertiesCount = this.propertyService.GetAllActiveUserPropertiesCount(this.UserId);
            var paginationModel = this.paginationService.CreatePagination(propertiesCount, PropertiesPerPage, page, this.ControllerName(nameof(MyPropertyController)), nameof(this.Index));
            var currentProperties = await this.propertyService.GetAllWithExpiredUserPropertiesPerPage(this.UserId, paginationModel.CurrentPage);

            this.ViewBag.Pager = paginationModel;
            this.ViewBag.IsFromMyProperties = true;
            return this.View(currentProperties);
        }

        [HttpGet]
        [Route("/MyProperties/ActiveProperties")]
        public async Task<IActionResult> ActiveProperties(int page = 1)
        {
            var propertiesCount = this.propertyService.GetAllActiveUserPropertiesCount(this.UserId);
            var paginationModel = this.paginationService.CreatePagination(propertiesCount, PropertiesPerPage, page, this.ControllerName(nameof(MyPropertyController)), nameof(this.ActiveProperties));
            var currentProperties = await this.propertyService.GetActiveUserPropertiesPerPageAsync(this.UserId, paginationModel.CurrentPage);

            this.ViewBag.Pager = paginationModel;
            this.ViewBag.IsFromExpired = false;
            this.ViewBag.IsFromMyProperties = true;
            this.cookie.Value = "/MyProperties/ActiveProperties";

            this.SetCookie(this.cookie.Name, this.cookie.Value, this.cookieOptions);
            return this.View(nameof(this.Index), currentProperties);
        }

        [HttpGet]
        [Route("/MyProperties/ExpiredProperties")]
        public async Task<IActionResult> ExpiredProperties(int page = 1)
        {
            var propertiesCount = this.propertyService.GetAllExpiredUserPropertiesCount(this.UserId);
            var paginationModel = this.paginationService.CreatePagination(propertiesCount, PropertiesPerPage, page, this.ControllerName(nameof(MyPropertyController)), nameof(this.ExpiredProperties));
            var currentProperties = await this.propertyService.GetExpiredUserPropertiesPerPageAsync(this.UserId, paginationModel.CurrentPage);

            this.ViewBag.Pager = paginationModel;
            this.ViewBag.IsFromExpired = true;
            this.ViewBag.IsFromMyProperties = true;

            this.cookie.Value = "/MyProperties/ExpiredProperties";

            this.SetCookie(this.cookie.Name, this.cookie.Value, this.cookieOptions);

            return this.View(nameof(this.Index), currentProperties);
        }

        [HttpPost]
        [Route("/MyProperties/RemoveUserProperty")]
        public async Task<IActionResult> RemoveUserProperty(int propertyId)
        {
            var isRemoved = await this.propertyService.RemoveByIdAsync(propertyId);

            if (isRemoved)
            {
                return this.Json(new { data = true });
            }

            return this.Json(new object { }); //TODO: Redirect to correct page;
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
