namespace RealEstate.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using RealEstate.Data.Models;
    using RealEstate.Services.Data.Interfaces;
    using RealEstate.Services.Messaging;
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
            var result = await this.signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                       
            return result;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel registerModel)
        {
            var user = new ApplicationUser
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
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
