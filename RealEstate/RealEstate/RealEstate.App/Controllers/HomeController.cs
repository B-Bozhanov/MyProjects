namespace RealEstate.App.Controllers
{
    using System.Diagnostics;
    using System.Text;

    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Models.ErrorViewModels;


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sales()
        {
            return this.View();
        }

        public IActionResult Rentals()
        {
            return this.View();
        }

        public IActionResult NewConstructions()
        {
            return this.View();
        }

        public IActionResult Agencies()
        {
            return this.View();
        }

        public IActionResult ImportProperties()
        {
            return this.View();
        }

        //[HttpPost]
        //public IActionResult ImportProperties(IFormFile file)
        //{
        //    if (file == null)
        //    {
        //        return this.Ok("File can no be null or empty!");
        //    }
        //    if (file.ContentType != "application/json")
        //    {
        //        return this.Ok("File extencion must be json");
        //    }

        //    var fileStr = this.GetFileAsString(file);

        //    this.importService.Import(fileStr);

        //    return this.RedirectToAction(nameof(Index));
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        private string GetFileAsString(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();

            return Encoding.UTF8.GetString(fileBytes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}