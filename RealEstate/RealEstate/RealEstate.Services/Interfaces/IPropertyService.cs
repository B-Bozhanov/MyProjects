namespace RealEstate.Services.Interfaces
{
    using RealEstate.Models.ImportModels;

    public interface IPropertyService
    {
        void Add(AddPropertyModel propertyModel);
    }
}
