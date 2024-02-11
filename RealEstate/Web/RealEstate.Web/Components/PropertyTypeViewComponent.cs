namespace RealEstate.Web.Components
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.Property;
    using RealEstate.Web.ViewModels.PropertyTypes;

    public class PropertyTypeViewComponent : ViewComponent
    {
        private readonly IPropertyTypeService propertyTypeService;

        public PropertyTypeViewComponent(IPropertyTypeService propertyTypeService)
        {
            this.propertyTypeService = propertyTypeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(BasePropertyModel model)
        {
            model.PropertyTypes = this.propertyTypeService.Get<PropertyTypeViewModel>();
            return this.View(model);
        }
    }
}
