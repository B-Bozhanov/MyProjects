//namespace RealEstate.Web.Controllers
//{
//    using Microsoft.AspNetCore.Mvc;

//    public class ImportPropertyController : Controller
//    {
//        private readonly IImportService importService;

//        public ImportPropertyController(IImportService importService)
//        {
//            this.importService = importService;
//        }

//        public IActionResult Import()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult Import(IFormFile file)
//        {
//            if (file == null)
//            {
//                return Ok("File can no be null or empty!");
//            }
//            if (file.ContentType != "application/json")
//            {
//                return Ok("File extencion must be json");
//            }

//            importService.Import(file);

//            return Redirect("/");
//        }
//    }
//}
