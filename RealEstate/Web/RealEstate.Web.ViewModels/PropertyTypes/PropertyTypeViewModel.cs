namespace RealEstate.Web.ViewModels.PropertyTypes
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class PropertyTypeViewModel : IMapFrom<PropertyType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
