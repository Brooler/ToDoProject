using Microsoft.AspNetCore.Mvc;

namespace ToDoProject.Web.Controllers.Web
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}