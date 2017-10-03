

using Microsoft.AspNetCore.Mvc;

namespace ToDoProject.Web.Controllers.Web
{
    public class AppController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}