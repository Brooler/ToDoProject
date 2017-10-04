using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUser(SignUpViewModel model);

        Task<SignInResult> LoginUser(LoginViewModel model);

        Task LogoutUser();
    }
}
