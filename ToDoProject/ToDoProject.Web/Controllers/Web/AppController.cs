using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ToDoProject.Web.Controllers.Web
{
    public class AppController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}