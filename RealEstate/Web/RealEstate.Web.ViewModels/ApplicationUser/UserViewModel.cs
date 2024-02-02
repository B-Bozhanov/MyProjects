namespace RealEstate.Web.ViewModels.ApplicationUser
{
    using RealEstate.Data.Models;
    using RealEstate.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string UserName { get; init; }

        public string PhoneNumber { get; init; }

        public string Email { get; init; }
    }
}
