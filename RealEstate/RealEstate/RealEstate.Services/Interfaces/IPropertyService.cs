namespace RealEstate.Services.Interfaces
{
    using RealEstate.Models.ImportViewModels;

    public interface IPropertyService
    {
        void Add(PropertyViewModel propertyModel);
    }
}
