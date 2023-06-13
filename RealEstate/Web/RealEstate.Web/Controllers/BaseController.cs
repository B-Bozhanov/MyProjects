namespace RealEstate.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BaseController : Controller
    {
        protected IActionResult RedirectToHome()
        {
            var method = nameof(HomeController.Index);
            var page = nameof(HomeController).Replace("Controller", string.Empty);

            return this.RedirectToAction(method, page);
        }
    }
}
