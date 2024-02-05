namespace RealEstate.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BaseController : Controller
    {
        protected IActionResult RedirectToHome()
        {
            var action = nameof(HomeController.Index);
            var controller = nameof(HomeController).Replace("Controller", string.Empty);

            return this.RedirectToAction(action, controller);
        }

        protected IActionResult RedirectToMyActiveProperties()
        {
            var action = nameof(MyPropertyController.ActiveProperties);
            var controller = nameof(MyPropertyController).Replace("Controller", string.Empty);

            return this.RedirectToAction(action, controller);
        }

        protected IActionResult RedirectToMyExpiredProperties()
        {
            var action = nameof(MyPropertyController.ExpiredProperties);
            var controller = nameof(MyPropertyController).Replace("Controller", string.Empty);

            return this.RedirectToAction(action, controller);
        }

        protected string ControllerName(string name)
        {
            var controllerName = name.Replace("Controller", string.Empty);

            return controllerName;
        }

        protected string UserId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
