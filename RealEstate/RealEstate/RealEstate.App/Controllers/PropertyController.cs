namespace RealEstate.App.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Models.DataModels;
    using RealEstate.Models.ImportModels;
    using RealEstate.Services.Interfaces;
    public class PropertyController : Controller
    {
        private readonly IImportService importService;
        private readonly IPropertyService propertyService;

        public PropertyController(IImportService importService, IPropertyService propertyService)
        {
            this.importService = importService;
            this.propertyService = propertyService;
        }

        public IActionResult Add()
        {
            return this.View(new AddPropertyModel
            {
                PropertyTypes = this.propertyService.GetPropertiesTypes()
                .Select(x => new PropertyTypeViewModel { Name = x.Name, Id = x.Id}),

                Places = this.propertyService.GetPlaces()
                .Select(x => new PlacesModel { Name = x.Name, Id = x.Id}),

                Districts = this.propertyService.GetDistricts()
                .Select(x => new DistrictsModel { Name = x.Name, Id = x.Id}),

                BuildingTypes = this.propertyService.GetBuildingsTypes()
                .Select(x => new BuildingTypeModel { Name = x.Name, Id = x.Id})
            }); 
        }

        [HttpPost]
        public IActionResult Add(AddPropertyModel property, IFormCollection form, List<BuildingTypeModel> buildingType)
        {
            // TODO validations:
            var images = form.Files;

            foreach (var item in form)
            {
            }

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

            return this.Redirect("/");
        }
    }
}
