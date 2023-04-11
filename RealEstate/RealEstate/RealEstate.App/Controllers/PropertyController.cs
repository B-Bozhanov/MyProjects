namespace RealEstate.App.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.App.Data;
    using RealEstate.Models.DataModels;
    using RealEstate.Models.ImportViewModels;
    using RealEstate.Services.Interfaces;
    public class PropertyController : Controller
    {
        private readonly IImportService importService;
        private readonly IPropertyService propertyService;
        private readonly ApplicationDbContext context;

        public PropertyController(IImportService importService, IPropertyService propertyService, ApplicationDbContext context)
        {
            this.importService = importService;
            this.propertyService = propertyService;
            this.context = context;
        }

        public IActionResult Add()
        {
            return this.View(new AddPropertyModel
            {
                PropertyTypes = this.GetPropertiesTypes(),
                Places = this.GetPlaces(),
                Districts = this.GetDistricts(),
            });
        }

        [HttpPost]
        public IActionResult Add(AddPropertyModel property, IFormCollection form)
        {
            // TODO validations:

            var images = form.Files;

            // TODO: Model binding
            // TODO: If there is no some of assigments!

            foreach (var imgFile in images)
            {
                var stream = new MemoryStream();
                imgFile.CopyTo(stream);

                var image = new Image { Name = imgFile.Name, Content = stream.ToArray() };

                property.Images!.Add(image);
            }

            this.propertyService.Add(property);

            return this.RedirectToAction(nameof(Index));
        }


        private IEnumerable<PropertyTypeViewModel> GetPropertiesTypes()
        {
            return context.PropertyTypes
                .Select(t => new PropertyTypeViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .OrderBy(t => t.Name)
                .ToList();
        }

        private IEnumerable<PlacesViewModel> GetPlaces()
        {
            return context.Places
                .Select(t => new PlacesViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .OrderBy(t => t.Name)
                .ToList();
        }

        private IEnumerable<DistrictsViewModel> GetDistricts()
        {
            return context.Districts
                .Select(t => new DistrictsViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .OrderBy(t => t.Name)
                .ToList();
        }
    }
}
