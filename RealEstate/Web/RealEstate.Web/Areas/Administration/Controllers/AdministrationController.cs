namespace RealEstate.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Common;
    using RealEstate.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
