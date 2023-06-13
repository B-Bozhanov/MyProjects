namespace RealEstate.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using RealEstate.Data.Models;
    using RealEstate.Web.ViewModels.Property;

    public interface IPropertyService
    {
        public int GetAllCount();

        public Task Add(AddPropertyInputModel propertyModel, ApplicationUser user, [CallerMemberName] string import = null!);

        public PropertyViewModel GetById(int id);

        public IEnumerable<PropertyViewModel> GetAllByOptionId(int optionId);

        public IEnumerable<PropertyViewModel> GetTopNewest(int count);

        public IEnumerable<PropertyViewModel> GetTopMostExpensive(int count);
    }
}
