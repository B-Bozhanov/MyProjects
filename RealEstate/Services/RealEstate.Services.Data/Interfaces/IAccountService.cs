namespace RealEstate.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using RealEstate.Web.ViewModels.Account;

    public interface IAccountService
    {
        public Task<IdentityResult> RegisterAsync(RegisterViewModel registerModel);

        public Task<SignInResult> LoginAsync(LoginViewModel loginModel);

        public Task SignOutAsync();
    }
}
