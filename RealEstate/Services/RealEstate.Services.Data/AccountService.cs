namespace RealEstate.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Web.ViewModels.Account;

    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel loginModel)
        {
            var user = await this.userManager.FindByNameAsync(loginModel.Username);

            if (user == null)
            {
                return null;
            }

            var result = await this.signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);

            return result;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel registerModel)
        {
            var user = new ApplicationUser
            {
                UserName = registerModel.Username,
                Email = registerModel.Email,
            };

            var result = await this.userManager.CreateAsync(user, registerModel.Password);

            return result;
        }

        public async Task SignOutAsync()
        {
            await this.signInManager.SignOutAsync();
        }
    }
}
