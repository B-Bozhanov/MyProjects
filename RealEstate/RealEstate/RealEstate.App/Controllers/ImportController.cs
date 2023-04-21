namespace RealEstate.App.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Interfaces;

    public class ImportController : Controller
    {
        private readonly IImportService importService;

        public ImportController(IImportService importService)
        {
            this.importService = importService;
        }

        public IActionResult Import()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            if (file == null)
            {
                return this.Ok("File can no be null or empty!");
            }
            if (file.ContentType != "application/json")
            {
                return this.Ok("File extencion must be json");
            }

            this.importService.Import(file);

            return this.Redirect("/");
        }
    }
}
