namespace RealEstate.Services.Interfaces
{
    using RealEstate.Models.ImportViewModels;

    public interface IPropertyService
    {
        void Add(AddPropertyModel propertyModel);
    }
}
