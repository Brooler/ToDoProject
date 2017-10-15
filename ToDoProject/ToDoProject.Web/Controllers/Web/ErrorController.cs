using Microsoft.AspNetCore.Mvc;

namespace ToDoProject.Web.Controllers.Web
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}