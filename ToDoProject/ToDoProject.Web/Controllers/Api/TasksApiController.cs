using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ToDoProject.Web.Repository;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Controllers.Api
{
    [Route("api/tasks")]
    [Authorize]
    public class TasksApiController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TasksApiController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUserTasks()
        {
            throw new NotImplementedException();
            //var result = await _taskRepository.GetAllUserTasks(string.Empty);

            //return Ok(result);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> AddEditTask(TaskAddEditViewModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPut("complete/{taskId}")]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            throw new NotImplementedException();
        }
    }
}