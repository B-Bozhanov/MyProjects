namespace RealEstate.App.Controllers
{
    using System.Diagnostics;
    using System.Text;

    using Microsoft.AspNetCore.Mvc;

    using RealEstate.App.Data;
    using RealEstate.App.Models;
    using RealEstate.Models.DataModels;
    using RealEstate.Models.ImportViewModels;
    using RealEstate.Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImportService importService;
        private readonly IPropertyService propertyService;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, IImportService importService, IPropertyService propertyService, ApplicationDbContext context)
        {
            _logger = logger;
            this.importService = importService;
            this.propertyService = propertyService;
            this.context = context;
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

        public IActionResult AddProperty()
        {
            return this.View(new PropertyViewModel
            {
                PropertyTypeViewModels = this.GetTypes()
            });
        }

        [HttpPost]
        public IActionResult AddProperty(IFormCollection form)
        {
            //TODO validations:

            var images = form.Files;

            // TODO: Model binding

            var importPropModel = new PropertyViewModel();

            foreach (var imgFile in images)
            {
                var stream = new MemoryStream();
                imgFile.CopyTo(stream);

                var image = new Image { Name = imgFile.Name, Content = stream.ToArray() };

                importPropModel.Images!.Add(image);
            }

            this.propertyService.Add(importPropModel);

            return this.View();
        }

        public IActionResult ImportProperties()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult ImportProperties(IFormFile file)
        {
            if (file == null)
            {
                return this.Ok("File can no be null or empty!");
            }
            if (file.ContentType != "application/json")
            {
                return this.Ok("File extencion must be json");
            }

            var fileStr = this.GetFileAsString(file);

            this.importService.Import(fileStr);

            return this.RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetFileAsString(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();

            return Encoding.UTF8.GetString(fileBytes);
        }

        private IEnumerable<PropertyTypeViewModel> GetTypes()
        {
            return context.PropertyTypes
                .Select(t => new PropertyTypeViewModel 
                { 
                    Id = t.Id, 
                    Name = t.Name 
                })
                .ToList();
        }
    }
}