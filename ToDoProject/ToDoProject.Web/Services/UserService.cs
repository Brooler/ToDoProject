using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ToDoProject.Web.ViewModels;
using AutoMapper;
using ToDoProject.Web.Models;

namespace ToDoProject.Web.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ProjectUser> _signInManager;
        private readonly UserManager<ProjectUser> _userManager;

        public UserService(SignInManager<ProjectUser> signInManager, UserManager<ProjectUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> LoginUser(LoginViewModel model)
        {
            var user = Mapper.Map<ProjectUser>(model);
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            return result;
        }

        public async Task<IdentityResult> RegisterUser(SignUpViewModel model)
        {
            var user = Mapper.Map<ProjectUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //TODO: Implement confirmation email sending

                await _signInManager.SignInAsync(user, true);
            }

            return result;
        }
    }
}
