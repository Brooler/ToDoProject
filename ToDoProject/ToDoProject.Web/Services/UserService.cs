using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ToDoProject.Web.ViewModels;
using AutoMapper;
using ToDoProject.Web.Models;
using ToDoProject.Business.Services;
using System.Security.Claims;

namespace ToDoProject.Web.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ProjectUser> _signInManager;
        private readonly UserManager<ProjectUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UserService(SignInManager<ProjectUser> signInManager, UserManager<ProjectUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> LoginUser(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
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

        public async Task LogoutUser()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GetCurrentUserId(ClaimsPrincipal user)
        {
            var projectUser = await _userManager.GetUserAsync(user);

            return projectUser.Id;
        }

        public async Task<ProjectUser> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
