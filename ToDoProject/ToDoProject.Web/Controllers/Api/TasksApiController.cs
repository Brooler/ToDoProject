using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Controllers.Api
{
    [Route("api/tasks")]
    public class TasksApiController : Controller
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetUserTasks()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task CreateTask(TaskAddEditViewModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("complete/{taskId}")]
        public async Task CompleteTask(int taskId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{taskId}")]
        public async Task DeleteTask(int taskId)
        {
            throw new NotImplementedException();
        }
    }
}