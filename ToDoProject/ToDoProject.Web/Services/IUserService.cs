using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoProject.Web.Models;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUser(SignUpViewModel model);

        Task<SignInResult> LoginUser(LoginViewModel model);

        Task LogoutUser();

        Task<string> GetCurrentUserId(ClaimsPrincipal user);

        Task<ProjectUser> GetUserById(string userId);
    }
}
