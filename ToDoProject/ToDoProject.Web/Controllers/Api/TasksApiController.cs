using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoProject.Web.Repository;
using ToDoProject.Web.Services;
using ToDoProject.Web.ViewModels;

namespace ToDoProject.Web.Controllers.Api
{
    [Route("api/tasks")]
    [Authorize]
    public class TasksApiController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserService _userService;

        public TasksApiController(ITaskRepository taskRepository, IUserService userService)
        {
            _taskRepository = taskRepository;
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUserTasks()
        {
            var userId = await _userService.GetCurrentUserId(User);
            var collection = await _taskRepository.GetAllUserTasks(userId);

            return Ok(collection);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            if (taskId == 0)
            {
                ModelState.AddModelError(string.Empty, "taskId can't be 0");
                return BadRequest();
            }
            var task = await _taskRepository.GetTask(taskId);

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditTask([FromBody]TaskAddEditViewModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }
            var userId = await _userService.GetCurrentUserId(User);
            model.UserId = userId;
            var completed = await _taskRepository.AddEditTask(model);
            if (completed)
            {
                completed = completed && await _taskRepository.SaveChanges();
            }

            if (completed)
            {
                return Ok();
            }
            ModelState.AddModelError(string.Empty, "Changes wasn't saved");

            return BadRequest();
        }

        [HttpPut("complete/{taskId}")]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            if(taskId == 0)
            {
                ModelState.AddModelError(string.Empty, "taskId can't be 0");

                return BadRequest();
            }

            var completed = await _taskRepository.CompleteTask(taskId);
            if (completed)
            {
                completed = completed && await _taskRepository.SaveChanges();
            }

            if (completed)
            {
                return Ok();
            }
            ModelState.AddModelError(string.Empty, "Something went wrong, changes wasn't saved");

            return BadRequest();
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            if (taskId == 0)
            {
                ModelState.AddModelError(string.Empty, "taskId can't be 0");

                return BadRequest();
            }

            var completed = await _taskRepository.DeleteTask(taskId);
            if (completed)
            {
                completed = completed && await _taskRepository.SaveChanges();
            }

            if (completed)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}